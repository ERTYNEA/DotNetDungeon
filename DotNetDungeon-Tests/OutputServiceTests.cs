namespace DotNetDungeon_Tests;

using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

[TestClass]
public sealed class OutputServiceTests
{
	private OutputService outputService = null!;
	private TextWriter originalConsoleOut = null!;
	private StringWriter stringWriter = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Create a new instance of the service
		outputService = new OutputService();

		// Capture the original console output
		originalConsoleOut = Console.Out;

		// Redirect console output to a StringWriter
		stringWriter = new StringWriter();
		Console.SetOut(stringWriter);
	}

	[TestCleanup]
	public void Cleanup()
	{
		// Dispose of the StringWriter
		stringWriter.Dispose();

		// Restore the original console output
		Console.SetOut(originalConsoleOut);
		originalConsoleOut = null!;

		// Clean up the instance of the service
		outputService = null!;
	}

	[TestMethod]
	public void PrintMatrix_ValidMatrix_PrintsCorrectly()
	{
		// Arrange
		char[,] matrix = new char[3, 3]
		{
			{ 'A', 'B', 'C' },
			{ 'D', 'E', 'F' },
			{ 'G', 'H', 'I' }
		};

		// Act
		outputService.PrintMatrix(matrix);

		// Assert
		string output = stringWriter.ToString();
		string expected = "ABC" + Environment.NewLine + "DEF" + Environment.NewLine + "GHI" + Environment.NewLine;
		output.Should().Be(expected);
	}

	[TestMethod]
	public void PrintMatrix_EmptyMatrix_PrintsNothing()
	{
		// Arrange
		char[,] matrix = new char[0, 0];

		// Act
		outputService.PrintMatrix(matrix);

		// Assert
		string output = stringWriter.ToString();
		output.Should().BeEmpty();
	}

	[TestMethod]
	public void PrintMatrix_NullMatrix_ThrowsException()
	{
		// Arrange
		char[,] matrix = null!;

		// Act
		Action act = () => outputService.PrintMatrix(matrix);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void PrintMatrix_LargeMatrix_PrintsCorrectly()
	{
		// Arrange
		int size = 10;
		char[,] matrix = new char[size, size];
		StringBuilder expectedBuilder = new StringBuilder();

		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				matrix[i, j] = (char)('0' + (i * size + j) % 10);
				expectedBuilder.Append(matrix[i, j]);
			}
			expectedBuilder.AppendLine();
		}

		// Act
		outputService.PrintMatrix(matrix);

		// Assert
		string output = stringWriter.ToString();
		string expected = expectedBuilder.ToString();

		// Compare line by line to handle potential newline differences
		string[] outputLines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
		string[] expectedLines = expected.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

		outputLines.Length.Should().Be(expectedLines.Length);
		for (int i = 0; i < expectedLines.Length; i++)
		{
			outputLines[i].Should().Be(expectedLines[i]);
		}
	}
}
