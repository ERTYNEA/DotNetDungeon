namespace DotNetDungeon_Services.Interfaces;

public interface IOutputService
{
	/// <summary>
	/// Prints a 2D character matrix to the console
	/// </summary>
	/// <param name="matrix">The character matrix to print</param>
	void PrintMatrix(char[,] matrix);
}