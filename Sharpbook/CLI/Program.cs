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
			Graf g = new Graf();
			Simpul a = new Simpul("a");
			a.AddBersisian("b");
			a.AddBersisian("c");
			Simpul b = new Simpul("b");
			b.AddBersisian("a");
			Simpul c = new Simpul("c");
			c.AddBersisian("a");
			g.AddSimpul(a);
			g.AddSimpul(b);
			g.AddSimpul(c);
			g.ShowGraf();
		}
	}
}
