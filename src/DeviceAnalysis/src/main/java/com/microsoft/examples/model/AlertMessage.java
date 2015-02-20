package com.microsoft.examples.model;

/*
 * Alert message
 */
public class AlertMessage {
	private String deviceId;
	private String deviceName;
	private boolean isActive;
	
	/*
	 * Get the Device Id
	 */
	public String getDeviceId(){
		return this.deviceId;
	}
	
	/*
	 * Set Device Id
	 */
	public void setDeviceId(String deviceId){
		this.deviceId = deviceId;
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
	public void setDeviceName(String deviceName){
		this.deviceName = deviceName;
	}
	
	/*
	 * True if device is active
	 */
	public boolean getIsActive(){
		return this.isActive;
	}
	
	/*
	 * Set is active
	 */
	public void setIsActive(boolean isActive){
		this.isActive = isActive;
	}
}
