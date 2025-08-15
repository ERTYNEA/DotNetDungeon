namespace DotNetDungeon_Services.Services;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using Spectre.Console;
using System;
using System.Globalization;

public class OutputService : IOutputService
{
	/// <summary>
	/// Converts a hexadecimal string to a Spectre.Console Color object.
	/// </summary>
	/// <param name="hexadecimalStringColor">Hexadecimal string color (format: #RRGGBB).</param>
	/// <returns>Spectre.Console Color object.</returns>
	public Color HexadecimalStringToColor(string hexadecimalStringColor)
	{
		// Verify that the string is not null or empty
		if (string.IsNullOrWhiteSpace(hexadecimalStringColor))
			throw new Exception();

		// If the string starts with '#', remove it
		if (hexadecimalStringColor[0] == '#')
			hexadecimalStringColor = hexadecimalStringColor.Substring(1);

		// Verify that the length is valid for RGB (6 characters)
		if (hexadecimalStringColor.Length != 6)
			throw new Exception();

		try
		{
			// Convert the hexadecimal parts to RGB values
			int r = int.Parse(hexadecimalStringColor.Substring(0, 2), NumberStyles.HexNumber);
			int g = int.Parse(hexadecimalStringColor.Substring(2, 2), NumberStyles.HexNumber);
			int b = int.Parse(hexadecimalStringColor.Substring(4, 2), NumberStyles.HexNumber);

			// Create and return the Color object
			return new Color((byte)r, (byte)g, (byte)b);
		}
		catch (FormatException)
		{
			throw new Exception();
		}
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
				// Get the TitleObject at the current position
				TitleObject titleObject = matrix[i, j];

				// Create a styled colors text with background
				var style = new Style()
					.Foreground(titleObject.CharacterColorText)
					.Background(titleObject.CharacterColorBackground);

				// Write the styled colors text with background
				AnsiConsole.Write(new Text(titleObject.CharacterChar.ToString(), style));
			}

			// Move to the next line
			AnsiConsole.WriteLine();
		}
	}
}