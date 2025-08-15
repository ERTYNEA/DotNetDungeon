using DotNetDungeon_Objets;

using Spectre.Console;

namespace DotNetDungeon_Services.Interfaces;

public interface IOutputService
{

	/// <summary>
	/// Converts a hexadecimal string to a Spectre.Console Color object.
	/// </summary>
	/// <param name="hexadecimalStringColor">Hexadecimal string color (format: #RRGGBB).</param>
	/// <returns>Spectre.Console Color object.</returns>
	Color HexadecimalStringToColor(string hexadecimalStringColor);

	/// <summary>
	/// Prints a 2D TitleObject matrix with colors.
	/// </summary>
	/// <param name="matrix">The TitleObject matrix to print.</param>
	void PrintMatrix(TitleObject[,] matrix);
}