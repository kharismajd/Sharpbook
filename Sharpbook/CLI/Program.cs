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
			Dictionary<string, List<string>> recommendation = new Dictionary<string, List<string>>();
			Graf graf;

			graf = file.getGraf();
			graf.ShowGraf();

			recommendation = graf.FriendRecommendation("A");
			Console.WriteLine("Daftar rekomendasi teman untuk akun A");
			foreach (string friend in recommendation.Keys)
            {
				Console.Write("Nama akun: ");
				Console.WriteLine(friend);
				Console.Write(recommendation[friend].Count);
				Console.WriteLine(" mutual friends", recommendation[friend].Count);
				foreach (string friendFromFriend in recommendation[friend])
                {
					Console.WriteLine(friendFromFriend);
                }
				Console.WriteLine("");
            }

            while (true)
            {
            }
		}
	}
}