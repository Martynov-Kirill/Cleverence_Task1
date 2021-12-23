using Cleverence_Task1.Server;

using System.Diagnostics;
using Cleverence_Task1.Server;
using Cleverence_Task1.Models;
using Cleverence_Task1.Models.Enums;
using System.Collections.Generic;
using System;

namespace Cleverence_Task1
{
	public class Program
	{
		private static readonly string[] Names ={
			"Gracie Jones", "Elianna Holmes", "Evelyn Hopkins",
			"Sarah Wright", "Kaylani Parker", "Destiny Thompson" ,
			"Natalia Torres"
		};

		static ManualResetEvent resetEvent = new ManualResetEvent(false);
		private static List<User> UsersGenerator(int size = 25)
		{
			Random random = new Random();
			List<User> users = new List<User>();

			for (int i = 0; i < size; i++)
				users.Add(new()
				{
					Id = i,
					Name = Names[random.Next(Names.Length - 1)],
					OperationType = random.Next(100) % 2 == 0 ? OperationType.Read : OperationType.Write,
				});

			return users;
		}

		static void Main(string[] args)
		{
			
			var collection = UsersGenerator();

			foreach (var user in collection)
				Parallel.Invoke(() =>
				{
					Thread operation = new(() => GetAction(user));
					operation.Start();
					Thread.Sleep(250);
				});

			Console.ReadLine();
		}

		private static async Task<bool> GetAction(User user)
		{
			if (user == null) 
				return await new Task<bool>(() => false);


			if (user.OperationType == OperationType.Write)
			{
				_ = WriteAction(user, resetEvent);
			}
			else
			{
				resetEvent.WaitOne();
				Thread.Sleep(250);

				_ = await ReadAction(user);
			}

			resetEvent.Reset();
			return await new Task<bool>(() => true); //Some Success response
		}

		private static Task<bool> WriteAction(User user, ManualResetEvent resetEvent)
		{

			DbContext.AddCount(1); // Do some work
			Console.WriteLine($"|{user.Id,-2:0#}| Write => {user.Name,-15} +1");

			ReadAction(user);

			resetEvent.Set();
			return new Task<bool>(() => true); //Some Success response
		}

		private static Task<bool> ReadAction(User user)
		{
			user.Value = DbContext.GetCount();
			Console.WriteLine($"|{user.Id,-2:0#}| Read  => {user.Name,-15}: {user.Value}");

			return new Task<bool>(() => true); //Some Success response
		}
	}
}