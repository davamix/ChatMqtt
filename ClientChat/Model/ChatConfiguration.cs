using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ClientChat.Model
{
	public class ChatConfiguration
	{
		public string Id { get; set; }
		public string ServerAddress { get; set; }
		public string[] Topics { get; set; }
		public byte[] QosLevels { get; set; }
	}
}
