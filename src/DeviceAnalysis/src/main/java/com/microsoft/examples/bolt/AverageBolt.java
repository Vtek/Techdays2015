package com.microsoft.examples.bolt;
import java.util.Vector;
import com.microsoft.examples.helper.DeviceNameHelper;
import com.microsoft.examples.model.AverageMessage;
import com.microsoft.examples.model.EventHubMessage;

import backtype.storm.topology.BasicOutputCollector;
import backtype.storm.topology.OutputFieldsDeclarer;
import backtype.storm.tuple.Tuple;

/*
 * Bolt dedicate to compute average
 */
public class AverageBolt extends SignalrBolt {
	
	private static final long serialVersionUID = 5906541986879739900L;
	
	private Vector<EventHubMessage> temperatures = new Vector<EventHubMessage>();
	private Vector<EventHubMessage> humidites = new Vector<EventHubMessage>();
	private Vector<EventHubMessage> sons = new Vector<EventHubMessage>();
	private Vector<EventHubMessage> luminosites = new Vector<EventHubMessage>();
	
	/*
	 * Get an average message for SignalR
	 */
	private AverageMessage GetAverage(Vector<EventHubMessage> messages)
	{
	  String deviceId = messages.size() > 0 ? messages.get(0).getDeviceId() : "";	 //deviceid 
		
	  double total = 0;
	  
	  for(int i = 0, l = messages.size(); i < l; i++){
		  total += Double.parseDouble(messages.get(i).getDatas());
	  }
	  
	  double average = total / (double)messages.size();
	  AverageMessage avrMsg = new AverageMessage();
	  avrMsg.setAverage(average);	  
	  avrMsg.setTimestamp(messages.get(messages.size() - 1).getTimestamp());
	  avrMsg.setDevice(deviceId);
	  avrMsg.setDeviceName(DeviceNameHelper.getName(deviceId));

      return avrMsg;
	}
	
	/*
	 * Execute the bolt
	 * @see backtype.storm.topology.IBasicBolt#execute(backtype.storm.tuple.Tuple, backtype.storm.topology.BasicOutputCollector)
	 */
	@Override
	public void execute(Tuple tuple, BasicOutputCollector output) {
      
		//add data to vector
		String deviceid = tuple.getStringByField("deviceid");
	    String datas = tuple.getStringByField("datas");
	    String timestamp = tuple.getStringByField("timestamp");
	      
	    EventHubMessage message = new EventHubMessage();
	    message.setDeviceId(deviceid);
	    message.setDatas(datas);
	    message.setTimestamp(timestamp);
		
	    if(DeviceNameHelper.isTemperature(deviceid)){
	    	temperatures.add(message);

	    	if(temperatures.size() == 10){
	    		AverageMessage temperatureMessage = GetAverage(temperatures);
	    		temperatures.clear();
	    		SendToSignalR("average", temperatureMessage);
	    	}
	    }
	    else if(DeviceNameHelper.isHumidite(deviceid)){
	    	humidites.add(message);
	    	
	    	if(humidites.size() == 10){
	    		AverageMessage humiditeMessage = GetAverage(humidites);
	    		humidites.clear();
	    		SendToSignalR("average", humiditeMessage);
	    	}
	    }
	    else if(DeviceNameHelper.isSon(deviceid)){
	    	sons.add(message);
	    	
	    	if(sons.size() == 10){
	    		AverageMessage sonMessage = GetAverage(sons);
	    		sons.clear();
	    		SendToSignalR("average", sonMessage);
	    	}
	    }
	    else if(DeviceNameHelper.isLuminosite(deviceid)){
	    	luminosites.add(message);
	    	
	    	if(luminosites.size() == 10){
	    		AverageMessage luminositeMessage = GetAverage(luminosites);
	    		luminosites.clear();
	    		SendToSignalR("average", luminositeMessage);
	    	}
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
