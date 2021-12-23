using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleverence_Task1.Server
{
	public static class DbContext
	{
		private static int Count { get; set; } = 0;

		public static int GetCount() => Count;

		public static Task<bool> AddCount(int value)
		{
			if (value > 0)
				Count += value;

			return new Task<bool>(() => true);
		}
	}
}
