package com.microsoft.examples.model;

public class EventHubMessage {
  private String timestamp;
  private String deviceid;
  private String datas;
  
  /*
   * Return the device identifier
   */
  public String getDeviceId() {
	  return this.deviceid;
  }
  
  /*
   * Set the device identifier
   */
  public void setDeviceId(String deviceid) {
	  this.deviceid = deviceid;
  }
  
  /*
   * Get the datas
   */
  public String getDatas(){
	  return this.datas;
  }
  
  /*
   * Set the datas
   */
  public void setDatas(String datas){
	  this.datas = datas;
  }
  
  /*
   * Get the timestamp
   */
  public String getTimestamp(){
	  return this.timestamp;
  }
  
  /*
   * Set the timestamp
   */
  public void setTimestamp(String timestamp){
	  this.timestamp = timestamp;
  }
}
