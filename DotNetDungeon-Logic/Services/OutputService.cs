namespace DotNetDungeon_Logic.Services;

using System;

public static class OutputService
{
	public static void PrintMatrix(char[,] matrix)
	{
		if (matrix == null)
			throw new Exception();

		int rows = matrix.GetLength(0);
		int columns = matrix.GetLength(1);

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
				Console.Write(matrix[i, j]);

			Console.WriteLine();
		}
	}
}