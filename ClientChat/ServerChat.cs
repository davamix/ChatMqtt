using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientChat
{
	/*
	 * This class manage all request from client to broker.
	 * It expose a MqttClient then the "thin clients" subscribe to the broker through this MqttClient
	 * It has a list of all client connected to the broker. 
	 */

	public class ServerChat
	{
		//TODO: Create an event for message published that fordward the mqttClient.MqttMsgPublishReceived event.
		private void MyMessageEventPublished()
		{
			//Fire event
			//TheEvent(message)
		}
		//This method will be in "thin client" process the event
		private void WhenEventFired(string message)
		{
			 //-----????
			//Option 1: Thin client manage the messages addressed to it.
			//			In this case if the thin client implementation does not make a filter by its topic could sniff all message, 
			//			not only the messages addressed to it.
			//Option 2: This server manage the message address and send to the correct thin client. 
			//			In this case the server should register a this client object.

		}
		



		public ServerChat()
		{
			//mqttClient.Subscribe("chat/main");
		}

		public void Subscribe(string username)
		{
			//_userList.Add(username);
			//mqttClient.Subscribe("chat/username");
		}

		public void Publish(string topic, string message)
		{

			//mqttClient.Publish(topic, message);
		}

	}
}
