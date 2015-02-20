package com.microsoft.examples.bolt;

import org.apache.log4j.Logger;

import backtype.storm.topology.base.BaseBasicBolt;
import backtype.storm.topology.BasicOutputCollector;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.tuple.Tuple;
import backtype.storm.tuple.Fields;
import backtype.storm.tuple.Values;

import com.google.gson.Gson;
import com.microsoft.examples.model.EventHubMessage;

/*
 * Bolt dedicate to parser the data and emit them into other bolt
 */
public class ParserBolt extends BaseBasicBolt {

	static Logger logger = Logger.getLogger(ParserBolt.class);
  static final long serialVersionUID = -1002427711518840509L;

  /*
   * Declare output fields & streams
   * @see backtype.storm.topology.IComponent#declareOutputFields(backtype.storm.topology.OutputFieldsDeclarer)
   */
  @Override
  public void declareOutputFields(OutputFieldsDeclarer declarer) {
	declarer.declareStream("hbasestream", new Fields("timestamp", "deviceid", "datas"));
    declarer.declareStream("dataDeviceStream", new Fields("timestamp", "deviceid", "datas"));
    declarer.declareStream("alertStream", new Fields("timestamp", "deviceid", "datas"));
    declarer.declareStream("averageStream", new Fields("timestamp", "deviceid", "datas"));
  }

  /*
   * Execute the bolt
   * @see backtype.storm.topology.IBasicBolt#execute(backtype.storm.tuple.Tuple, backtype.storm.topology.BasicOutputCollector)
   */
  @Override
  public void execute(Tuple tuple, BasicOutputCollector collector) {
	//String tuple value from ServiceBus(it's a JSON message :))
    String value = tuple.getString(0);
    logger.debug(value);

    String[] arr = value.split("}");
    for (String ehm : arr)
    {
        EventHubMessage msg = new Gson().fromJson(ehm.concat("}"),EventHubMessage.class);

        String timestamp = msg.getTimestamp();
        String deviceid = msg.getDeviceId();
        String datas = msg.getDatas();
        
        //emit data to all bolt
        collector.emit("hbasestream", new Values(timestamp, deviceid, datas));
        collector.emit("dataDeviceStream", new Values(timestamp, deviceid, datas));
        collector.emit("alertStream", new Values(timestamp, deviceid, datas));
        collector.emit("averageStream", new Values(timestamp, deviceid, datas));
    }
  }
}
