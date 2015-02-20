package com.microsoft.examples.model;

/*
 * Device data message
 */
public class DeviceDataMessage {
  private String device;
  private String deviceName;
  private String datas;
  private String timestamp;
  private String unit;
  
  /*
   * Return the device identifier
   */
  public String getDevice() {
	  return this.device;
  }
  
  /*
   * Set the device identifier
   */
  public void setDevice(String device) {
	  this.device = device;
  }
  
  /*
   * Get the device name
   */
  public String getDeviceName(){
	  return this.deviceName;
  }
  
  /*
   * Set the device name
   */
  public void setDeviceName(String deviceName) {
	  this.deviceName = deviceName;
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
  
  /*
   * Get the unit
   */
  public String getUnit(){
	  return this.unit;
  }
  
  /*
   * Set the unit
   */
  public void setUnit(String unit){
	  this.unit = unit;
  }
}
