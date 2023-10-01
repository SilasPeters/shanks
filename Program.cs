using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanks
{
	static class Program
	{
		private static BigInteger p;
		private static BigInteger g;
		private static BigInteger q;
		private static int        k;
		private static BigInteger w;
		private static BigInteger h;

		static void Main(string[] args)
		{
			 p = BigInteger.Parse(Console.ReadLine()); // Max 318 numbers, modulus
			 g = BigInteger.Parse(Console.ReadLine()); // Max 318 numbers, base
			 q = BigInteger.Parse(Console.ReadLine()); // Max 60 numbers, ord
			 k = int.Parse(Console.ReadLine());        // Max 6 numbers, amount of numbers to test
			 w = FastSquareRoot(q / k);
			 h = BigInteger.ModPow(g, w, p);

			Dictionary<BigInteger, BigInteger> bosjesboek = CreateBosjesBoek();

			Span<BigInteger> xs = new BigInteger[k];
			for (int i = 0; i < k; i++)
				xs[i] = int.Parse(Console.ReadLine().TakeWhile(x => x != ' ').ToArray()); // Can contain a dirty suffix

			for (int i = 0; i < k; i++)
			{
				BigInteger result = DiscreteLog(xs[i], ref bosjesboek);
				Console.WriteLine(result == -1 ? "geen macht" : result.ToString());
			}
		}

		static BigInteger FastSquareRoot(BigInteger bigInteger)
		{
			long n = bigInteger.GetBitLength();
			return bigInteger >> (int)(n / 2);
		}

		static Dictionary<BigInteger, BigInteger> CreateBosjesBoek()
		{
			Dictionary<BigInteger, BigInteger> bosjesboek = new Dictionary<BigInteger, BigInteger>();

			BigInteger power = 1;
			bosjesboek.Add(1, 0);
			BigInteger aantalHopmanStappen = q / w;
			for (BigInteger i = 1; i < aantalHopmanStappen + 1; i++)
				bosjesboek.Add(power = power * h % p, i * w);

			return bosjesboek;
		}

		static BigInteger DiscreteLog(BigInteger x, ref Dictionary<BigInteger, BigInteger> bosjesboek)
		{
			for (BigInteger i = 0; i < w; i++, x = x * g % p)
				if (bosjesboek.TryGetValue(x, out BigInteger macht))
					return macht - i;
			return -1;
		}
	}
}
