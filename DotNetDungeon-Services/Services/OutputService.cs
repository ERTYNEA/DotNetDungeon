namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using System;
using System.Drawing;
using Console = Colorful.Console;

public class OutputService : IOutputService
{
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
				// Get the TitleObject at the current position
				TitleObject titleObject = matrix[i, j];

				// Set the background color and print the character
				Console.BackgroundColor = titleObject.CharacterColorBackground;
				Console.Write(titleObject.CharacterChar.ToString(), titleObject.CharacterColorText);
			}

			// Reset background color for new line
			Console.BackgroundColor = Color.Black;

			// Move to the next line
			Console.WriteLine();
		}
	}
}