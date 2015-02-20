package com.microsoft.examples;

import backtype.storm.Config;
import backtype.storm.LocalCluster;
import backtype.storm.StormSubmitter;
import backtype.storm.generated.StormTopology;
import backtype.storm.topology.TopologyBuilder;
import backtype.storm.tuple.Fields;

import com.microsoft.eventhubs.spout.EventHubSpout;
import com.microsoft.eventhubs.spout.EventHubSpoutConfig;
import com.microsoft.examples.bolt.AlertBolt;
import com.microsoft.examples.bolt.AverageBolt;
import com.microsoft.examples.bolt.DeviceDataBolt;
import com.microsoft.examples.bolt.ParserBolt;

import java.io.FileReader;
import java.util.Properties;

import org.apache.storm.hbase.bolt.mapper.SimpleHBaseMapper;
import org.apache.storm.hbase.bolt.HBaseBolt;

import java.util.Map;
import java.util.HashMap;

public class DeviceAnalysis
{
  protected EventHubSpoutConfig spoutConfig;
  protected int numWorkers;

  // Reads the configuration information for the Event Hub spout
  protected void readEHConfig(String[] args) throws Exception {
    Properties properties = new Properties();
    if(args.length > 1) {
      properties.load(new FileReader(args[1]));
    }
    else {
      properties.load(DeviceAnalysis.class.getClassLoader().getResourceAsStream(
        "Config.properties"));
    }

    String username = properties.getProperty("eventhubspout.username");
    String password = properties.getProperty("eventhubspout.password");
    String namespaceName = properties.getProperty("eventhubspout.namespace");
    String entityPath = properties.getProperty("eventhubspout.entitypath");
    String zkEndpointAddress = properties.getProperty("zookeeper.connectionstring");
    int partitionCount = Integer.parseInt(properties.getProperty("eventhubspout.partitions.count"));
    int checkpointIntervalInSeconds = Integer.parseInt(properties.getProperty("eventhubspout.checkpoint.interval"));
    int receiverCredits = Integer.parseInt(properties.getProperty("eventhub.receiver.credits"));
    System.out.println("Eventhub spout config: ");
    System.out.println("  partition count: " + partitionCount);
    System.out.println("  checkpoint interval: " + checkpointIntervalInSeconds);
    System.out.println("  receiver credits: " + receiverCredits);
    spoutConfig = new EventHubSpoutConfig(username, password,
      namespaceName, entityPath, partitionCount, zkEndpointAddress,
      checkpointIntervalInSeconds, receiverCredits);

    //set the number of workers to be the same as partition number.
    //the idea is to have a spout and a partial count bolt co-exist in one
    //worker to avoid shuffling messages across workers in storm cluster.
    numWorkers = spoutConfig.getPartitionCount();
    
    if(args.length > 0) {
      //set topology name so that sample Trident topology can use it as stream name.
      spoutConfig.setTopologyName(args[0]);
    }
  }

  // Create the spout using the configuration
  protected EventHubSpout createEventHubSpout() {
    EventHubSpout eventHubSpout = new EventHubSpout(spoutConfig);
    return eventHubSpout;
  }

  // Build the topology
  protected StormTopology buildTopology(EventHubSpout eventHubSpout, SimpleHBaseMapper mapper) {
    TopologyBuilder topologyBuilder = new TopologyBuilder();
    
    // Name the spout 'EventHubsSpout', and set it to create
    // as many as we have partition counts in the config file
    topologyBuilder.setSpout("EventHub", eventHubSpout, spoutConfig.getPartitionCount())
      .setNumTasks(spoutConfig.getPartitionCount());
    
    // Create the parser bolt, which subscribes to the stream from EventHub
    topologyBuilder.setBolt("Parser", new ParserBolt(), spoutConfig.getPartitionCount())
      .localOrShuffleGrouping("EventHub").setNumTasks(spoutConfig.getPartitionCount());
    
    topologyBuilder.setBolt("DeviceData", new DeviceDataBolt(), spoutConfig.getPartitionCount())
      .fieldsGrouping("Parser", "dataDeviceStream", new Fields("timestamp", "deviceid", "datas")).setNumTasks(spoutConfig.getPartitionCount());
    
    topologyBuilder.setBolt("Average", new AverageBolt(), spoutConfig.getPartitionCount())
      .fieldsGrouping("Parser", "averageStream", new Fields("timestamp", "deviceid", "datas")).setNumTasks(1);
    
    topologyBuilder.setBolt("Alert", new AlertBolt(), spoutConfig.getPartitionCount())
      .fieldsGrouping("Parser", "alertStream", new Fields("timestamp", "deviceid", "datas")).setNumTasks(spoutConfig.getPartitionCount());
    
    // Create the HBase bolt, which subscribes to the stream from Parser
    topologyBuilder.setBolt("HBase", new HBaseBolt("SensorData", mapper).withConfigKey("hbase.conf"), spoutConfig.getPartitionCount())
      .fieldsGrouping("Parser", "hbasestream", new Fields("timestamp", "deviceid", "datas")).setNumTasks(spoutConfig.getPartitionCount());
    
    return topologyBuilder.createTopology();
  }

  protected void submitTopology(String[] args, StormTopology topology, Config config) throws Exception {
    config.setDebug(true);
    config.registerMetricsConsumer(backtype.storm.metric.LoggingMetricsConsumer.class, 1);
    config.setNumWorkers(numWorkers);
    StormSubmitter.submitTopology(args[0], config, topology);
  }

  // Loads the configuration, creates the spout, builds the topology,
  // and then submits it
  protected void runScenario(String[] args) throws Exception{
    readEHConfig(args);
    Config config = new Config();

    //hbase configuration
    Map<String, Object> hbConf = new HashMap<String, Object>();
    if(args.length > 0) {
      hbConf.put("hbase.rootdir", args[0]);
    }
    config.put("hbase.conf", hbConf);
    SimpleHBaseMapper mapper = new SimpleHBaseMapper()
          .withRowKeyField("timestamp")
          .withColumnFields(new Fields("deviceid", "datas"))
          .withColumnFamily("cf");

    EventHubSpout eventHubSpout = createEventHubSpout();
    StormTopology topology = buildTopology(eventHubSpout, mapper);
    submitTopology(args, topology, config);
  }

  public static void main(String[] args) throws Exception {
    DeviceAnalysis scenario = new DeviceAnalysis();
    scenario.runScenario(args);
  }
}
