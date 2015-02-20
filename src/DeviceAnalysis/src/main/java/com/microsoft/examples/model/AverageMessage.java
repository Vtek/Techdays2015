package com.microsoft.examples.model;

/*
 * Average message for SignalR
 */
public class AverageMessage {
	  private String device;
	  private String deviceName;
	  private double average;
	  private String timestamp;
	  
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
	   * Get the average
	   */
	  public double getAverage(){
		  return this.average;
	  }
	  
	  /*
	   * Set the average
	   */
	  public void setAverage(double average){
		  this.average = average;
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
