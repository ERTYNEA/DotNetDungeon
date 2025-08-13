namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using System;
using System.Drawing;
using Console = Colorful.Console;

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
	/// Prints a 2D TitleObject matrix with colors.
	/// </summary>
	/// <param name="matrix">The TitleObject matrix to print.</param>
	public void PrintMatrix(TitleObject[,] matrix)
	{
		// Validate the input matrix
		if (matrix == null)
			throw new Exception();

		// Get the dimensions of the matrix
		int rows = matrix.GetLength(0);
		int columns = matrix.GetLength(1);

		// Print the matrix with colors
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < columns; j++)
			{
				TitleObject titleObj = matrix[i, j];
				Console.BackgroundColor = titleObj.CharacterColorBackground;
				Console.Write(titleObj.CharacterChar.ToString(), titleObj.CharacterColorText);
			}

			Console.BackgroundColor = Color.Black; // Reset background color for new line
			Console.WriteLine();
		}
	}
}