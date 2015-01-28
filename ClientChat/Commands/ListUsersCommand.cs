using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientChat.Commands
{
	public class ListUsersCommand:ICommand
	{
		private readonly List<string> _userList;
		public string Id { get; private set; }

		public ListUsersCommand(List<string> userList)
		{
			_userList = userList;
			Id = "L";
		}

		public void Run()
		{
			_userList.ForEach(Console.WriteLine);
		}
	}
}
