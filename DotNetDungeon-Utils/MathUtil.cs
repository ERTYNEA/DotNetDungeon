namespace DotNetDungeon_Utils;

using System;

public static class MathUtil
{
	public static int GenerateRandomInteger(int min, int max)
	{
		if (min > max)
			throw new Exception();

		Random random = new Random();

		return random.Next(min, max + 1);
	}
}