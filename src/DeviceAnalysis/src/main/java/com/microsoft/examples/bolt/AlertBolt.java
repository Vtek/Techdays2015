package com.microsoft.examples.bolt;

import com.microsoft.examples.helper.DeviceNameHelper;
import com.microsoft.examples.model.AlertMessage;

import backtype.storm.topology.BasicOutputCollector;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.tuple.Tuple;

/*
 * Bolt dedicate to Alert
 */
public class AlertBolt extends SignalrBolt {

	private static final long serialVersionUID = 9113790935463183397L;

	/*
	 * Execute the bolt
	 * @see backtype.storm.topology.IBasicBolt#execute(backtype.storm.tuple.Tuple, backtype.storm.topology.BasicOutputCollector)
	 */
	@Override
	public void execute(Tuple tuple, BasicOutputCollector collector) {
		String deviceId = tuple.getStringByField("deviceid");
		
		if(DeviceNameHelper.isInterrupteur(deviceId) || DeviceNameHelper.isPorte(deviceId)){
			boolean isActive = "1".equals(tuple.getStringByField("datas"));
			
			AlertMessage message = new AlertMessage();
			message.setDeviceId(deviceId);
			message.setDeviceName(DeviceNameHelper.getName(deviceId));
			message.setIsActive(isActive);
			
			SendToSignalR("alert", message);
		}
	}

	/*
	 * Declare output fields & streams
	 * @see backtype.storm.topology.IComponent#declareOutputFields(backtype.storm.topology.OutputFieldsDeclarer)
	 */
	@Override
	public void declareOutputFields(OutputFieldsDeclarer declarer) {
		
	}

}
