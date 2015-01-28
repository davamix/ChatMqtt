using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClientChat.Commands;
using ClientChat.Model;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ClientChat
{
	class Program
	{
		private static MqttClient _client;
		private static ChatConfiguration _configuration;
		private static List<string> _userList;
		private static List<ICommand> _commands;


		static void Main(string[] args)
		{
			_userList = new List<string>();

			Initialize();

			StartClient();

			_client.Disconnect();

		}

		/// <summary>
		/// Create the client configration and connect to broker
		/// </summary>
		/// TODO: Move to ServerChat class
		private static void Initialize()
		{
			_configuration = new ChatConfiguration();

			_commands = RegisterCommands();
			Configure();

			_client = new MqttClient(_configuration.ServerAddress);

			_client.MqttMsgPublishReceived += ShowMessage;
			_client.ConnectionClosed += (sender, args) => Environment.Exit(0);
			_client.MqttMsgSubscribed += SubscribeUsername;
			_client.MqttMsgUnsubscribed += UnsubscribeUsername;

			_client.Connect(_configuration.Id);
			_client.Subscribe(_configuration.Topics, _configuration.QosLevels);
		}

		//TODO: Move to ServerChat class
		private static List<ICommand> RegisterCommands()
		{
			return new List<ICommand>
			       {
				       new ListUsersCommand(_userList)
			       };
		}

		//TODO: Move to ServerChat class
		private static void UnsubscribeUsername(object sender, MqttMsgUnsubscribedEventArgs args)
		{
			_userList.Remove(((MqttClient) sender).ClientId);
		}

		//TODO: Move to ServerChat class
		private static void SubscribeUsername(object sender, MqttMsgSubscribedEventArgs args)
		{
			_userList.Add(((MqttClient)sender).ClientId);
		}


		/// <summary>
		/// Show message addressed to a personal chat or to main chat according to the topic
		/// [0] - main chat | [1] - personal chat
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void ShowMessage(object sender, MqttMsgPublishEventArgs e)
		{
			Console.WriteLine(e.Topic.Equals(_configuration.Topics[1]) 
				? "## {0}"  //Meesage to personal chat
				: ">> {0}" //Message to main chat
				, Encoding.UTF8.GetString(e.Message));
		}


		/// <summary>
		/// Establish the attributes for connect to the broker.
		/// </summary>
		/// TODO: Move to ServerChat class
		private static void Configure()
		{
			Console.WriteLine("Configuring...");
			//_idClient = Guid.NewGuid().ToString();	
			_configuration.Id = SetUserName();
			_configuration.ServerAddress = "192.168.1.101";

			_configuration.Topics = new[]
			                        {
				                        "chat/main",
				                        string.Format("chat/{0}", _configuration.Id)
			                        };

			_configuration.QosLevels = new[]
			                           {
				                           MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE,
				                           MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE
			                           };
		}

		/// <summary>
		/// The ID for this client is the user name, not a Guid.
		/// </summary>
		/// <returns></returns>
		private static string SetUserName()
		{
			Console.Write("Write your user name: ");
			return Console.ReadLine();
		}


		/// <summary>
		/// Input for messages.
		/// </summary>
		/// TODO: Change the methdo name
		/// TODO: Move to ServerChat class
		private static void StartClient()
		{
			var message = string.Empty;

			do
			{
				Console.Write("> ");
				message = Console.ReadLine();

				ProcessCommand(message);
			} while (!message.Equals("exit"));

		}
		
		//TODO: Move to ServerChat class
		private static void ProcessCommand(string message)
		{
			if (message.StartsWith("$"))
				SendMessage(message, GetRoom(message));

			if (message.StartsWith("/"))
				_commands.First(x=>x.Id.Equals(message[1])).Run();

			SendMessage(message, "main");
		}

		//TODO: Move to ServerChat class
		private static void SendMessage(string message, string room)
		{
			var topic = string.Format("chat/{0}", room);

			_client.Publish(topic, Encoding.UTF8.GetBytes(message));
		}

		//TODO: Move to ServerChat class
		private static string GetRoom(string message)
		{
			return message.Substring(1, message.IndexOf("#") - 1);
		}
	}
}
