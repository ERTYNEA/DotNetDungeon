using DotNetDungeon_Objets;

namespace DotNetDungeon_Services.Interfaces;

public interface IOutputService
{
	/// <summary>
	/// Prints a 2D TitleObject matrix with colors.
	/// </summary>
	/// <param name="matrix">The TitleObject matrix to print.</param>
	void PrintMatrix(TitleObject[,] matrix);
}