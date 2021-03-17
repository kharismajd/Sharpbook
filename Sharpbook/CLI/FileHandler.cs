using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
	class FileHandler
	{
		public Graf getGraf()
		{
			string fileName;
			string line;
			string[] fileSimpuls;

			int counter;
			int edgesCount;

			Graf graf = new Graf();
			Simpul simpul;

			Console.Write("Masukkan nama file: ");

			fileName = Console.ReadLine();
			System.IO.StreamReader file = new System.IO.StreamReader("../../test/" + fileName);

			line = file.ReadLine();
			edgesCount = Int16.Parse(line);

			counter = 0;
			while (counter < edgesCount)
			{

				line = file.ReadLine();
				fileSimpuls = line.Split(' ');

				if (graf.GetSimpuls().Find(simp => simp.GetNama() == fileSimpuls[0]) == null)
				{
					simpul = new Simpul(fileSimpuls[0]);
					simpul.AddBersisian(fileSimpuls[1]);
					graf.AddSimpul(simpul);
				}
				else
				{
					graf.GetSimpuls().Find(simp => simp.GetNama() == fileSimpuls[0]).AddBersisian(fileSimpuls[1]);
				}

				if (graf.GetSimpuls().Find(simp => simp.GetNama() == fileSimpuls[1]) == null)
				{
					simpul = new Simpul(fileSimpuls[1]);
					simpul.AddBersisian(fileSimpuls[0]);
					graf.AddSimpul(simpul);
				}
				else
				{
					graf.GetSimpuls().Find(simp => simp.GetNama() == fileSimpuls[1]).AddBersisian(fileSimpuls[0]);
				}

				counter++;
			}

			foreach (Simpul simp in graf.GetSimpuls())
			{
				simp.sortBersisian();
			}

			return graf;
		}
	}
}