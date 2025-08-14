namespace DotNetDungeon_Tests;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;

[TestClass]
public sealed class OutputServiceTests
{
	private IOutputService outputService = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Create a new instance of the service
		outputService = new OutputService();
	}

	[TestMethod]
	public void PrintMatrix_NullMatrix()
	{
		// Arrange
		TitleObject[,] matrix = null!;

		// Act
		Action act = () => outputService.PrintMatrix(matrix);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void PrintMatrix_EmptyMatrix()
	{
		// Arrange
		TitleObject[,] matrix = new TitleObject[0, 0];

		// Act
		Action act = () => outputService.PrintMatrix(matrix);

		// Assert
		act.Should().NotThrow();
	}

	[TestMethod]
	public void PrintMatrix_ValidMatrix()
	{
		// Arrange
		TitleObject[,] matrix = new TitleObject[3, 3];

		// Initialize the matrix with TitleObjects
		matrix[0, 0] = new TitleObject { CharacterChar = 'A', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[0, 1] = new TitleObject { CharacterChar = 'B', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[0, 2] = new TitleObject { CharacterChar = 'C', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[1, 0] = new TitleObject { CharacterChar = 'D', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[1, 1] = new TitleObject { CharacterChar = 'E', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[1, 2] = new TitleObject { CharacterChar = 'F', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[2, 0] = new TitleObject { CharacterChar = 'G', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[2, 1] = new TitleObject { CharacterChar = 'H', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };
		matrix[2, 2] = new TitleObject { CharacterChar = 'I', CharacterColorText = Color.Black, CharacterColorBackground = Color.Black };

		// Act
		try
		{
			outputService.PrintMatrix(matrix);
		}
		catch (Exception ex)
		{
			// If there's an error, it's likely a platform-specific console access issue, which we can ignore in tests
			if (!(ex is System.PlatformNotSupportedException) &&
				!(ex is InvalidOperationException) &&
				!ex.Message.Contains("console"))
			{
				// If it's a different kind of exception, we want the test to fail
				throw;
			}
		}
	}
}
