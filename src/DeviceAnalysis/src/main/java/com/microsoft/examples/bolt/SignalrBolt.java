package com.microsoft.examples.bolt;

import java.util.Map;

import microsoft.aspnet.signalr.client.Action;
import microsoft.aspnet.signalr.client.ErrorCallback;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler;

import com.google.gson.Gson;
import com.microsoft.examples.model.ConnectionMessage;

import backtype.storm.task.TopologyContext;
import backtype.storm.topology.base.BaseBasicBolt;

public abstract class SignalrBolt extends BaseBasicBolt  {

	private static final long serialVersionUID = -8265923052807934978L;

	private HubConnection conn;
	private HubProxy proxy;
	
	/*
	 * Prepare the SignalR connection and proxy
	 * @see backtype.storm.topology.base.BaseBasicBolt#prepare(java.util.Map, backtype.storm.task.TopologyContext)
	 */
	@Override
	public void prepare(Map config, TopologyContext context) {
		conn = new HubConnection("MyDashBoardUrl");
		proxy = conn.createHubProxy("DashHub");
		conn.error(new ErrorCallback() {
			@Override
			public void onError(Throwable error) {
				error.printStackTrace();
			}
		});

		conn.connected(new Runnable() {
			@Override
			public void run() {
				System.out.println("CONNECTED");
				ConnectionMessage message = new ConnectionMessage();
				message.setMessage("CONNECTED");
				SendToSignalR("information", message);
			}
		});
		
		conn.reconnected(new Runnable() {
			@Override
			public void run() {
				System.out.println("RECONNECTED");
				ConnectionMessage message = new ConnectionMessage();
				message.setMessage("RECONNECTED");
				SendToSignalR("information", message);
			}
		});

		conn.closed(new Runnable() {
			@Override
			public void run() {
				System.out.println("CLOSED");
			}	
		});

		conn.start().done(new Action<Void>() {
			@Override
			public void run(Void obj) throws Exception {
				System.out.println("STARTED");
			}
		});
		
		proxy.on("ping",new SubscriptionHandler() {
			
			@Override
			public void run() {
				ConnectionMessage message = new ConnectionMessage();
				message.setMessage("pong");
				SendToSignalR("information", message);
			}
		});
	}
	
	/*
	 * Send data to SignalR
	 */
	public void SendToSignalR(String action, Object obj)
	{
		  Gson gson = new Gson();
	      proxy.invoke(action, gson.toJson(obj));
	}
}
