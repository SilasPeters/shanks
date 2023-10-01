using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanks
{
	public static class Program
	{
		private static BigInteger modulus;
		private static BigInteger @base;
		private static BigInteger order;
		private static int        numberOfKeys;
		private static BigInteger bigStepSize;
		private static BigInteger bigStepValue;

		public static void Main(string[] args)
		{
			modulus      = BigInteger.Parse(Console.ReadLine()); // Max 318 numbers, modulus
			@base        = BigInteger.Parse(Console.ReadLine()); // Max 318 numbers, base
			order        = BigInteger.Parse(Console.ReadLine()); // Max 60 numbers, order
			numberOfKeys = int.Parse(Console.ReadLine());        // Max 6 numbers, amount of keys to crack
			bigStepSize  = FastSquareRoot(order / numberOfKeys);
			bigStepValue = BigInteger.ModPow(@base, bigStepSize, modulus);

			Dictionary<BigInteger, BigInteger> index = CreateIndex();

			Span<BigInteger> keys = new BigInteger[numberOfKeys];
			for (int i = 0; i < numberOfKeys; i++)
				keys[i] = int.Parse(Console.ReadLine().TakeWhile(c => c != ' ').ToArray());

			for (int i = 0; i < numberOfKeys; i++)
			{
				BigInteger result = DiscreteLog(keys[i], ref index);
				Console.WriteLine(result != -1 ? result.ToString() : "geen macht");
			}
		}

		static BigInteger FastSquareRoot(BigInteger bigInteger)
		{
			long n = bigInteger.GetBitLength();
			return bigInteger >> (int)(n / 2); // TODO using int might be a problem
		}

		static Dictionary<BigInteger, BigInteger> CreateIndex()
		{
			Dictionary<BigInteger, BigInteger> index = new Dictionary<BigInteger, BigInteger>();

			index.Add(1, 0);
			BigInteger power = 1;
			BigInteger bigSteps = order / bigStepSize;
			for (BigInteger i = 1; i < bigSteps + 1; i++)
				index.Add(power = power * bigStepValue % modulus, i * bigStepSize);

			return index;
		}

		static BigInteger DiscreteLog(BigInteger x, ref Dictionary<BigInteger, BigInteger> index)
		{
			for (BigInteger smallSteps = 0; smallSteps < bigStepSize; smallSteps++, x = x * @base % modulus)
				if (index.TryGetValue(x, out BigInteger exponent)) {
					var result = exponent - smallSteps;
					return result >= 0 ? result : order - smallSteps;
					// When the result is negative, this means we found the exponent 0 in the index. Thus, to find
					// the discrete log, we need to subtract the smallSteps from the order of the group instead of 0.
				}
			return -1;
		}
	}
}
