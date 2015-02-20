package com.microsoft.examples.helper;

/*
 * Helper dedicate to device & name
 */
public class DeviceNameHelper {
	//Device Identifiers
	private static String temperatureDeviceId = "180bbc2-1";
	private static String humiditeDeviceId = "180bbc2-2";
	private static String sonDeviceId = "4-5";
	private static String luminositeDeviceId = "4-6";
	private static String porteDeviceId = "1806f82-4";
	private static String interrupteurDeviceId = "258651-3";
	
	/*
	 * Retreive device name corresponding to a deviceId
	 */
	public static String getName(String deviceId){
		if(isTemperature(deviceId)){
			return "Température";
		}
		else if(isHumidite(deviceId)){
			return "Humidité";
		}
		else if(isSon(deviceId)){
			return "Son";
		}
		else if(isLuminosite(deviceId)){
			return "Luminosité";
		}
		else if(isPorte(deviceId)){
			return "Porte";
		}
		else if(isInterrupteur(deviceId)){
			return "Interrupteur";
		}
		else{
			return "Unknow";
		}
	}
	
	/*
	 * Retreive device measure unit corresponding to a deviceId
	 */
	public static String getMeasureUnit(String deviceId){
		if(isTemperature(deviceId)){
			return "°C";
		}
		else if(isHumidite(deviceId)){
			return "%";
		}
		else if(isSon(deviceId)){
			return "db";
		}
		else if(isLuminosite(deviceId)){
			return "lux";
		}
		else if(isPorte(deviceId)){
			return "";
		}
		else if(isInterrupteur(deviceId)){
			return "";
		}
		else{
			return "";
		}
	}
	
	/*
	 * True if the device id corresponding to the temperature device
	 */
	public static boolean isTemperature(String deviceId){
		return temperatureDeviceId.equals(deviceId);
	}
	
	/*
	 * True if the device id corresponding to the humidite device
	 */
	public static boolean isHumidite(String deviceId){
		return humiditeDeviceId.equals(deviceId);
	}
	
	/*
	 * True if the device id corresponding to the son device
	 */
	public static boolean isSon(String deviceId){
		return sonDeviceId.equals(deviceId);
	}
	
	/*
	 * True if the device id corresponding to the luminosite device
	 */
	public static boolean isLuminosite(String deviceId){
		return luminositeDeviceId.equals(deviceId);
	}
	
	/*
	 * True if the device id corresponding to the porte device
	 */
	public static boolean isPorte(String deviceId){
		return porteDeviceId.equals(deviceId);
	}
	
	/*
	 * True if the device id corresponding to the interrupteur device
	 */
	public static boolean isInterrupteur(String deviceId){
		return interrupteurDeviceId.equals(deviceId);
	}
}
