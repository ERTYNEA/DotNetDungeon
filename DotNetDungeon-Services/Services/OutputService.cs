namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using System;

public class OutputService : IOutputService
{
	/// <summary>
	/// Prints a 2D character matrix to the console
	/// </summary>
	/// <param name="matrix">The character matrix to print</param>
	public void PrintMatrix(char[,] matrix)
	{
		// Validate the input matrix
		if (matrix == null)
			throw new Exception();

		// Get the dimensions of the matrix
		int rows = matrix.GetLength(0);
		int columns = matrix.GetLength(1);

		// Print the matrix to the console
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
				Console.Write(matrix[i, j]);

			Console.WriteLine();
		}
	}
}