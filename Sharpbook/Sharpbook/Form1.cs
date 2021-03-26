using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sharpbook
{
	public partial class Form1 : Form
	{
		private Graf graf;
		private Dictionary<string, List<string>> friendRecommend;
		private String friendRecommendationNode;
		private String pathNode1;
		private String pathNode2;
		private List<string> path;
		private Microsoft.Msagl.Drawing.Graph graf_visual;


		public Form1()
		{
			InitializeComponent();
		}

		public void getGraf(string fileName)
		{
			string line;
			string[] fileSimpuls;

			int counter;
			int edgesCount;

			Simpul simpul;
			Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();

			this.graf = new Graf();
			this.graf_visual = new Microsoft.Msagl.Drawing.Graph("graf");

			System.IO.StreamReader file = new System.IO.StreamReader(fileName);

			line = file.ReadLine();
			edgesCount = Int16.Parse(line);

			counter = 0;
			while (counter < edgesCount)
			{
				// Bagian kelas graf
				line = file.ReadLine();
				fileSimpuls = line.Split(' ');

				if (this.graf.GetSimpulByName(fileSimpuls[0]) == null)
				{
					simpul = new Simpul(fileSimpuls[0]);
					simpul.AddBersisian(fileSimpuls[1]);
					this.graf.AddSimpul(simpul);
				}
				else
				{
					this.graf.GetSimpulByName(fileSimpuls[0]).AddBersisian(fileSimpuls[1]);
				}

				if (this.graf.GetSimpulByName(fileSimpuls[1]) == null)
				{
					simpul = new Simpul(fileSimpuls[1]);
					simpul.AddBersisian(fileSimpuls[0]);
					this.graf.AddSimpul(simpul);
				}
				else
				{
					this.graf.GetSimpulByName(fileSimpuls[1]).AddBersisian(fileSimpuls[0]);
				}

				// Bagian kelas visualisasi graf
				var edge = this.graf_visual.AddEdge(fileSimpuls[0], fileSimpuls[1]);
				this.graf_visual.FindNode(fileSimpuls[0]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightCyan;
				this.graf_visual.FindNode(fileSimpuls[1]).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightCyan;
				edge.Attr.Color = Microsoft.Msagl.Drawing.Color.LightBlue;
				edge.Attr.ArrowheadAtSource = Microsoft.Msagl.Drawing.ArrowStyle.None;
				edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;

				counter++;
			}

			foreach (Simpul simp in this.graf.GetSimpuls())
			{
				simp.sortBersisian();
			}

			// Masukkan graf ke viewer
			viewer.Graph = this.graf_visual;
			viewer.Name = "graf";
			viewer.OutsideAreaBrush = Brushes.White;
			viewer.ToolBarIsVisible = false;
			viewer.Dock = System.Windows.Forms.DockStyle.Fill;

			// Masukkan viewer ke form
			this.SuspendLayout();
			this.panel1.Controls.Add(viewer);
			this.ResumeLayout();
		}

		public void printFriendRecommendation()
        {
			String output = "";
			output += "Daftar rekomendasi teman untuk akun ";
			output += this.friendRecommendationNode;
			output += " :\r\n";
			foreach (string friend in this.friendRecommend.Keys)
			{
				output += "Nama akun: ";
				output += friend;
				output += "\r\n";
				output += this.friendRecommend[friend].Count;
				output += " mutual friends\r\n";
				foreach (string friendFromFriend in this.friendRecommend[friend])
				{
					output += friendFromFriend;
					output += "\r\n";
				}
				output += "\r\n";
			}
			this.richTextBox1.Text = output;
		}

		public void printPath()
        {
			String output = "";
			output += "Nama akun: ";
			output += this.pathNode1;
			output += " dan ";
			output += this.pathNode2;
			output += "\r\n";
			if (this.path == null)
            {
				output += "Tidak ada jalur koneksi yang tersedia\r\n";
				output += "Anda harus memulai koneksi baru itu sendiri";
            }
			else if (this.pathNode1 == this.pathNode2)
			{
				output += "Kamu sudah berteman dengan dirimu sendiri";
			}
			else if (this.path.Count() == 2)
            {
				output += "Kamu sudah berteman dengan orang tersebut";
            }
			else
            {
				if (this.path.Count() == 3)
                {
					output += "1st-";
                }
				else if (this.path.Count() == 4)
                {
					output += "2nd-";
                }
				else if (this.path.Count() == 5)
                {
					output += "3rd-";
                }
				else
                {
					output += this.path.Count - 2;
					output += "th-";
                }
				output += "degree connection\r\n";
				output += this.path[0];
				
				for (int i = 1; i < this.path.Count(); i++)
                {
					output += " -> ";
					output += this.path[i];
                }
            }
			this.richTextBox1.Text = output;
        }

        private void button1_Click(object sender, EventArgs e)
        {
			OpenFileDialog openFileDialog1 = new OpenFileDialog
			{
				InitialDirectory = @"C:\",
				Title = "Browse",

				CheckFileExists = true,
				CheckPathExists = true,
				DefaultExt = "txt",
				Filter = "txt files|*.txt",
				FilterIndex = 2,
				RestoreDirectory = true
			};

			DialogResult result = openFileDialog1.ShowDialog();
			if (result == DialogResult.OK)
			{
				this.SuspendLayout();
				this.panel1.Controls.Clear();
				this.ResumeLayout();

				string file = openFileDialog1.FileName;

				try
				{
					getGraf(file);
					List<Simpul> simpuls = this.graf.GetSimpuls();
					foreach (Simpul simpul in simpuls)
					{
						comboBox2.Items.Add(simpul.GetNama());
						comboBox3.Items.Add(simpul.GetNama());
					}
					this.label2.Text = Path.GetFileName(file);
				}
				catch
				{
					MessageBox.Show("Format file tidak sesuai", "Error");
					this.label2.Text = "";
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			friendRecommendationNode = comboBox2.SelectedItem.ToString();
			this.friendRecommend = this.graf.FriendRecommendation(friendRecommendationNode);
			this.printFriendRecommendation();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			pathNode1 = comboBox2.SelectedItem.ToString();
			pathNode2 = comboBox3.SelectedItem.ToString();
			string algoritma = comboBox1.SelectedItem.ToString();
			if (algoritma == "BFS")
			{
				this.path = this.graf.BFS(pathNode1, pathNode2);
			}
			if (algoritma == "DFS")
            {
				this.path = this.graf.DFS(pathNode1, pathNode2);
			}
			this.printPath();
		}

		private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

		}

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

		}

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Graf
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

			foreach (string friend in rootFriends)
			{
				friendFromFriends = GetSimpulByName(friend).GetBersisian();
				foreach (string friendFromFriend in friendFromFriends)
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
		public List<string> BFS(string s, string e)
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
				return lintasan;
			}
			else 
			{
				Console.WriteLine("Tidak ditemukan");
				return null;
			}
		}
		public List<string> DFS(string s, string e)
		{
			Dictionary<string, bool> visited = new Dictionary<string, bool>();
			List<string> path = new List<string>();
			for (int i = 0; i < this.GetNSimpul(); i++)
			{
				visited.Add(this.GetSimpuls()[i].GetNama(), false);
			}
			return DFSHandler(visited, path, s, e);

		}
		public List<string> DFSHandler(Dictionary<string, bool> visited, List<string> path, string at, string finish)
		{
			visited[at] = true;
			if (at == finish)
			{
				path.Add(at);
				return path;
			}

			foreach (string calon in GetSimpulByName(at).GetBersisian())
			{
				if (!visited[calon])
				{
					List<string> tempPath = new List<string>();
					foreach (string astg in path) { tempPath.Add(astg); }
					tempPath.Add(at);
					List<string> temp = DFSHandler(visited, tempPath, calon, finish);
					if (temp != null)
					{
						return temp;
					}
				}
			}
			return null;
		}
	}

	public class Simpul
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
