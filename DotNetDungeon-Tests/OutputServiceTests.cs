namespace DotNetDungeon_Tests;

using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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
		char[,] matrix = null!;

		// Act
		Action act = () => outputService.PrintMatrix(matrix);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void PrintMatrix_EmptyMatrix()
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
	public void PrintMatrix_ValidMatrix()
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
}
