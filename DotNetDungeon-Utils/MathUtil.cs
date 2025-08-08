namespace DotNetDungeon_Utils;

using System;

public static class MathUtil
{
	// TODO - COPILOT: We need to put a summary here that indicates what this method does
	public static int GenerateRandomInteger(int min, int max)
	{
		// TODO - COPILOT: We need to explain what this part of the code does
		if (min > max)
			throw new Exception();

		// TODO - COPILOT: We need to explain what this part of the code does
		Random random = new Random();

		// TODO - COPILOT: We need to explain what this part of the code does
		return random.Next(min, max + 1);
	}
}
