using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cleverence_Task1.Models.Enums;

namespace Cleverence_Task1.Models
{
	public class User
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public int Value { get; set; }

		public OperationType OperationType { get; set; }
	}
}
