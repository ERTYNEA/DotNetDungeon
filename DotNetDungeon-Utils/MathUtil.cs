namespace DotNetDungeon_Utils;

using System;

public static class MathUtil
{
	/// <summary>
	/// Generates a random integer within the specified inclusive range [min, max]
	/// </summary>
	/// <param name="min">The minimum value (inclusive)</param>
	/// <param name="max">The maximum value (inclusive)</param>
	/// <returns>A random integer between min and max</returns>
	public static int GenerateRandomInteger(int min, int max)
	{
		// Validate input parameters to ensure min is not greater than max
		if (min > max)
			throw new Exception();

		// Create a new Random instance for generating random numbers
		Random random = new Random();

		// Return a random number between min (inclusive) and max (inclusive)
		return random.Next(min, max + 1);
	}
}
