namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Services.Interfaces;
using System;

public class OutputService : IOutputService
{
	private readonly TextWriter writer;

	/// <summary>
	/// Initializes a new instance of the <see cref="OutputService"/> class.
	/// </summary>
	/// <param name="writer">The text writer to use for output.</param>
	public OutputService(TextWriter writer)
	{
		this.writer = writer;
	}

	/// <summary>
	/// Prints a 2D character matrix.
	/// </summary>
	/// <param name="matrix">The character matrix to print.</param>
	public void PrintMatrix(char[,] matrix)
	{
		// Validate the input matrix
		if (matrix == null)
			throw new Exception();

		// Get the dimensions of the matrix
		int rows = matrix.GetLength(0);
		int columns = matrix.GetLength(1);

		// Print the matrix
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
				writer.Write(matrix[i, j]);

			writer.WriteLine();
		}
	}
}