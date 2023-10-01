using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Shanks
{
	public static class Program
	{
		private static BigInteger modulus;      // Max 318 numbers
		private static BigInteger @base;        // Max 318 numbers
		private static BigInteger order;        // Max 60 numbers
		private static int        numberOfKeys; // Max 6 numbers
		private static BigInteger bigStepExponent;
		private static BigInteger bigStepValue;

		public static void Main(string[] args)
		{
			char[] legalCharacters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			modulus         = BigInteger.Parse(Console.ReadLine().TakeWhile(c => legalCharacters.Contains(c)).ToArray());
			@base           = BigInteger.Parse(Console.ReadLine().TakeWhile(c => legalCharacters.Contains(c)).ToArray());
			order           = BigInteger.Parse(Console.ReadLine().TakeWhile(c => legalCharacters.Contains(c)).ToArray());
			numberOfKeys    = int.Parse(Console.ReadLine()       .TakeWhile(c => legalCharacters.Contains(c)).ToArray());
			bigStepExponent = FastSquareRoot(ref order, ref numberOfKeys);
			bigStepValue    = BigInteger.ModPow(@base, bigStepExponent, modulus);

			// Key: power, Value: exponent
			Dictionary<BigInteger, BigInteger> index = CreateIndex();

			BigInteger[] keys = new BigInteger[numberOfKeys];
			for (int i = 0; i < numberOfKeys; i++)
				keys[i] = BigInteger.Parse(Console.ReadLine().TakeWhile(c => c != ' ').ToArray());

			for (int i = 0; i < numberOfKeys; i++)
			{
				DiscreteLog(ref keys[i], ref index);
				Console.WriteLine(keys[i] != -1 ? keys[i].ToString() : "geen macht");
			}
		}

		static BigInteger FastSquareRoot(ref BigInteger orderRef, ref int numberOfKeysRef)
		{
			BigInteger bigInteger = orderRef / numberOfKeysRef;
			int        bits = 0;
			BigInteger i    = 1;
			while (i << bits < bigInteger) { bits += 1; }
			return bigInteger >> (bits / 2);
		}

		static Dictionary<BigInteger, BigInteger> CreateIndex()
		{
			Dictionary<BigInteger, BigInteger> index = new Dictionary<BigInteger, BigInteger>();

			index.Add(1, 0);
			BigInteger power = 1;
			BigInteger bigSteps = order / bigStepExponent;
			for (BigInteger i = 1; i < bigSteps + 1; i++)
				index.Add(power = power * bigStepValue % modulus, i * bigStepExponent);

			return index;
		}

		static void DiscreteLog(ref BigInteger x, ref Dictionary<BigInteger, BigInteger> index)
		{
			for (BigInteger smallSteps = 0; smallSteps < bigStepExponent; smallSteps++, x = x * @base % modulus)
				if (index.TryGetValue(x, out BigInteger exponent)) {
					var result = exponent - smallSteps;
					x = result >= 0 ? result : order - smallSteps;
					// When the result is negative, this means we found the exponent 0 in the index. Thus, to find
					// the discrete log, we need to subtract the smallSteps from the order of the group instead of 0.
				}
			x = -1;
		}
	}
}
