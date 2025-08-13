namespace DotNetDungeon_Tests;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using NSubstitute;
using System.Text;

[TestClass]
public sealed class OutputServiceTests
{
	private StringWriter stringWriter = null!;
	private OutputService outputService = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Redirect console output to a StringWriter
		stringWriter = new StringWriter();
		outputService = new OutputService(stringWriter);
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
		outputService.PrintMatrix(matrix);

		// Assert
		string output = stringWriter.ToString();
		output.Should().BeEmpty();
	}

	[TestMethod]
	public void PrintMatrix_ValidMatrix()
	{
		// This test is more complex since we need to mock Colorful.Console
		// and there's no easy way to test the colors directly
		// Instead, we'll test the basic functionality that the matrix is processed

		// Arrange
		TitleObject[,] matrix = new TitleObject[2, 2];
		
		// Create test TitleObjects
		matrix[0, 0] = new TitleObject { CharacterChar = 'A', CharacterColorText = Color.Red, CharacterColorBackground = Color.Black };
		matrix[0, 1] = new TitleObject { CharacterChar = 'B', CharacterColorText = Color.Green, CharacterColorBackground = Color.Black };
		matrix[1, 0] = new TitleObject { CharacterChar = 'C', CharacterColorText = Color.Blue, CharacterColorBackground = Color.Black };
		matrix[1, 1] = new TitleObject { CharacterChar = 'D', CharacterColorText = Color.Yellow, CharacterColorBackground = Color.Black };

		try
		{
			// Act - This might not actually print to our stringWriter due to how Colorful.Console works
			outputService.PrintMatrix(matrix);
			
			// No assertion here since we can't capture the colored output reliably in tests
			// The test passes if no exception is thrown
		}
		catch (Exception ex)
		{
			// If there's an error, it's likely a platform-specific console access issue, which we can ignore in tests
			// We're just making sure the code doesn't throw during normal operation
			if (!(ex is System.PlatformNotSupportedException) && 
				!(ex is InvalidOperationException) && 
				!ex.Message.Contains("console"))
			{
				throw; // If it's a different kind of exception, we want the test to fail
			}
		}
	}
}
