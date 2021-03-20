using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
	class Graf
	{
		/*--- Fields ---*/
		List<Simpul> simpuls; // adjacency list
		int nSimpul; // jumlah simpul

		/*--- Methods ---*/
		// default constructor
		public Graf()
		{
			this.simpuls = new List<Simpul>();
			this.nSimpul = 0;
		}

		// copy constructor
		public Graf(Graf _graf)
		{
			this.simpuls = new List<Simpul>();
			for (int i = 0; i < _graf.nSimpul; i++)
			{
				this.simpuls.Add(_graf.simpuls[i]);
			}
			this.nSimpul = _graf.nSimpul;
		}

		// menampilkan info graf
		public void ShowGraf()
		{
			if (nSimpul == 0)
			{
				Console.WriteLine("Graf kosong");
			}
			else 
			{
				for (int i = 0; i < nSimpul; i++)
				{
					this.simpuls[i].ShowSimpul();
				}
			}
		}

		// menambahkan simpul ke dalam graf
		public void AddSimpul(Simpul _simp)
		{
			this.simpuls.Add(_simp);
			this.nSimpul++;
		}

		// menghapus simpul dari graf dan mengembalikan simpul yang dihapus
		public Simpul DelSimpul(string _namaSimp)
		{
			int i = 0;
			while (i < this.nSimpul && this.simpuls[i].GetNama() != _namaSimp) { i++; }

			Simpul _simp = this.simpuls[i];

			this.simpuls.RemoveAt(i);
			nSimpul--;

			return _simp;
		}

		public Simpul GetSimpulByName(string simpul)
		{
			return this.simpuls.Find(simp => simp.GetNama() == simpul);
		}

		public List<Simpul> GetSimpuls()
		{
			return this.simpuls;
		}

		public int GetNSimpul() { return this.nSimpul; }
		public Dictionary<string, List<string>> FriendRecommendation(string namaSimpul)
		{
			Dictionary<string, List<string>> recommendation = new Dictionary<string, List<string>>();
			Dictionary<string, List<string>> sortedRecommendation = new Dictionary<string, List<string>>();
			List<string> rootFriends;
			List<string> friendFromFriends;
			Simpul root;

			root = GetSimpulByName(namaSimpul);
			rootFriends = root.GetBersisian();

			foreach(string friend in rootFriends)
			{
				friendFromFriends = GetSimpulByName(friend).GetBersisian();
				foreach(string friendFromFriend in friendFromFriends)
				{
					if (!rootFriends.Contains(friendFromFriend) && friendFromFriend != namaSimpul)
					{
						if (recommendation.ContainsKey(friendFromFriend))
						{
							recommendation[friendFromFriend].Add(friend);
						}
						else
						{
							List<string> mutualFriends = new List<string>();
							mutualFriends.Add(friend);
							recommendation.Add(friendFromFriend, mutualFriends);
						}
					}
				}
			}
			sortedRecommendation = recommendation.OrderByDescending(x => x.Value.Count).ToDictionary(x => x.Key, x => x.Value);
			return sortedRecommendation;
		}
		public void BFS(string s, string e)
		{

			Queue<string> queue = new Queue<string>();
			Dictionary<string, bool> visited = new Dictionary<string, bool>();
			Dictionary<string, string> previous = new Dictionary<string, string>();
			for (int i = 0; i < this.GetNSimpul(); i++)
			{
				visited.Add(this.GetSimpuls()[i].GetNama(), false);
				previous.Add(this.GetSimpuls()[i].GetNama(), "");
			}
			queue.Enqueue(s);
			visited[s] = true;
			while (queue.Any())
			{
				String dequeued = queue.Dequeue();
				Simpul simp = this.GetSimpulByName(dequeued);
				foreach (string calon in simp.GetBersisian())
				{
					if (!visited[calon])
					{
						queue.Enqueue(calon);
						visited[calon] = true;
						previous[calon] = dequeued;
					}
				}
			}

			List<string> lintasan = new List<string>();
			for (string x = e; x != ""; x = previous[x])
			{
				lintasan.Add(x);
			}
			lintasan.Reverse();
			if (lintasan[0] == s)
			{
				foreach (string i in lintasan) { Console.WriteLine(i); }
			}
			else { Console.WriteLine("Tidak ditemukan"); }

		}
	}

	class Simpul
	{
		/*--- Fields ---*/
		string nama; // nama simpul
		List<string> bersisian; // list simpul yang bersisian
		int nBersisian; // jumlah simpul yang bersisian

		/*--- Methods ---*/
		// ctor dengan parameter nama and bersisian
		public Simpul(string _nama)
		{
			this.nama = _nama;
			this.bersisian = new List<string>();
			this.nBersisian = 0;
		}

		// getter
		public string GetNama()
		{
			return this.nama;
		}

		// menambahkan simpul yang bersisian
		public void AddBersisian(string _namaB)
		{
			this.bersisian.Add(_namaB);
			this.nBersisian++;
		}

		public void sortBersisian()
		{
			this.bersisian.Sort();
		}

		public List<string> GetBersisian()
		{
			return this.bersisian;
		}

		// menampilkan info simpul
		public void ShowSimpul()
		{
			Console.Write(this.nama);
			Console.Write(": ");
			if (nBersisian != 0)
			{
				for (int i = 0; i < nBersisian; i++)
				{
					Console.Write(this.bersisian[i]);
					if (i != nBersisian - 1)
					{
						Console.Write(", ");
					}
				}
				Console.WriteLine();
			}
			else { Console.WriteLine("-"); }
		}
	}
}
