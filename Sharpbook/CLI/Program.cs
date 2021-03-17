using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			FileHandler file = new FileHandler();
			Graf graf;

			graf = file.getGraf();
			graf.ShowGraf();

			while(true)
            {

            }
		}
	}
}