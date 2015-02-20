package com.microsoft.examples.bolt;

import backtype.storm.topology.BasicOutputCollector;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.tuple.Tuple;
import com.microsoft.examples.helper.DeviceNameHelper;
import com.microsoft.examples.model.DeviceDataMessage;


/*
 * Bolt dedice to device data
 */
public class DeviceDataBolt extends SignalrBolt {

	private static final long serialVersionUID = -2351232908436693618L;

	/*
	 * Declare output fields & streams
	 * @see backtype.storm.topology.IComponent#declareOutputFields(backtype.storm.topology.OutputFieldsDeclarer)
	 */
	@Override
	public void declareOutputFields(OutputFieldsDeclarer declarer) {
	
	}

  
	/*
	 * Execute the bolt
	 * @see backtype.storm.topology.IBasicBolt#execute(backtype.storm.tuple.Tuple, backtype.storm.topology.BasicOutputCollector)
	 */
	@Override
    public void execute(Tuple tuple, BasicOutputCollector collector) {
      try {

        String deviceid = tuple.getStringByField("deviceid");
        String datas = tuple.getStringByField("datas");
        String timestamp = tuple.getStringByField("timestamp");
      
        DeviceDataMessage message = new DeviceDataMessage();
        message.setDevice(deviceid);
        message.setDatas(datas);
        message.setTimestamp(timestamp);
        message.setDeviceName(DeviceNameHelper.getName(deviceid));
        message.setUnit(DeviceNameHelper.getMeasureUnit(deviceid));

        SendToSignalR("send", message);
        
      } catch (Exception e) {
       collector.reportError(e);
    }
  }
}