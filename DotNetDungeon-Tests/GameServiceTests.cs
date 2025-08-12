namespace DotNetDungeon_Tests;

using FluentAssertions;
using DotNetDungeon_Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class GameServiceTests
{
	private GameService gameService = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Create a new instance of the service
		gameService = new GameService();
	}

	[TestMethod]
	public void GenerateDungeonLevel_ReturnsMatrixWithCorrectDimensions()
	{
		// Arrange
		int height = 25;
		int width = 80;
		int roomNumberForLevelMin = 5;
		int roomNumberForLevelMax = 10;
		int roomHeightMin = 5;
		int roomHeightMax = 10;
		int roomWidthMin = 5;
		int roomWidthMax = 10;
		char nothingChar = ' ';
		char wallChar = '#';
		char floorChar = '.';

		// Act
		var result = gameService.GenerateDungeonLevel(
			height,
			width,
			roomNumberForLevelMin,
			roomNumberForLevelMax,
			roomHeightMin,
			roomHeightMax,
			roomWidthMin,
			roomWidthMax,
			nothingChar,
			wallChar,
			floorChar);

		// Assert
		result.Should().NotBeNull();
		result.GetLength(0).Should().Be(height);
		result.GetLength(1).Should().Be(width);
	}

	public void GenerateDungeonLevel_MatrixHasNothingChar()
	{
		// Arrange
		int height = 25;
		int width = 80;
		int roomNumberForLevelMin = 5;
		int roomNumberForLevelMax = 10;
		int roomHeightMin = 5;
		int roomHeightMax = 10;
		int roomWidthMin = 5;
		int roomWidthMax = 10;
		char nothingChar = ' ';
		char wallChar = '#';
		char floorChar = '.';

		// Act
		var result = gameService.GenerateDungeonLevel(
			height,
			width,
			roomNumberForLevelMin,
			roomNumberForLevelMax,
			roomHeightMin,
			roomHeightMax,
			roomWidthMin,
			roomWidthMax,
			nothingChar,
			wallChar,
			floorChar);

		// Assert
		bool hasNothingChar = false;
		for (int y = 0; y < height && !hasNothingChar; y++)
			for (int x = 0; x < width && !hasNothingChar; x++)
				if (result[y, x] == ' ')
					hasNothingChar = true;

		hasNothingChar.Should().BeTrue();
	}

	public void GenerateDungeonLevel_MatrixHasWallChar()
	{
		// Arrange
		int height = 25;
		int width = 80;
		int roomNumberForLevelMin = 5;
		int roomNumberForLevelMax = 10;
		int roomHeightMin = 5;
		int roomHeightMax = 10;
		int roomWidthMin = 5;
		int roomWidthMax = 10;
		char nothingChar = ' ';
		char wallChar = '#';
		char floorChar = '.';

		// Act
		var result = gameService.GenerateDungeonLevel(
			height,
			width,
			roomNumberForLevelMin,
			roomNumberForLevelMax,
			roomHeightMin,
			roomHeightMax,
			roomWidthMin,
			roomWidthMax,
			nothingChar,
			wallChar,
			floorChar);

		// Assert
		bool hasWallChar = false;
		for (int y = 0; y < height && !hasWallChar; y++)
			for (int x = 0; x < width && !hasWallChar; x++)
				if (result[y, x] == wallChar)
					hasWallChar = true;

		hasWallChar.Should().BeTrue();
	}

	[TestMethod]
	public void GenerateDungeonLevel_MatrixHasFloorChar()
	{
		// Arrange
		int height = 25;
		int width = 80;
		int roomNumberForLevelMin = 5;
		int roomNumberForLevelMax = 10;
		int roomHeightMin = 5;
		int roomHeightMax = 10;
		int roomWidthMin = 5;
		int roomWidthMax = 10;
		char nothingChar = ' ';
		char wallChar = '#';
		char floorChar = '.';

		// Act
		var result = gameService.GenerateDungeonLevel(
			height,
			width,
			roomNumberForLevelMin,
			roomNumberForLevelMax,
			roomHeightMin,
			roomHeightMax,
			roomWidthMin,
			roomWidthMax,
			nothingChar,
			wallChar,
			floorChar);

		// Assert
		bool hasFloorChar = false;
		for (int y = 0; y < height && !hasFloorChar; y++)
			for (int x = 0; x < width && !hasFloorChar; x++)
				if (result[y, x] == floorChar)
					hasFloorChar = true;

		hasFloorChar.Should().BeTrue();
	}

	[TestMethod]
	public void GenerateDungeonLevel_ZeroRoomsBoundsCheck()
	{
		// Arrange
		int height = 25;
		int width = 80;
		int roomNumberForLevelMin = 0;
		int roomNumberForLevelMax = 0;
		int roomHeightMin = 5;
		int roomHeightMax = 10;
		int roomWidthMin = 5;
		int roomWidthMax = 10;
		char nothingChar = ' ';
		char wallChar = '#';
		char floorChar = '.';

		// Act
		var result = gameService.GenerateDungeonLevel(
			height,
			width,
			roomNumberForLevelMin,
			roomNumberForLevelMax,
			roomHeightMin,
			roomHeightMax,
			roomWidthMin,
			roomWidthMax,
			nothingChar,
			wallChar,
			floorChar);

		// Assert
		bool hasWallChar = false;
		bool hasFloorChar = false;
		for (int y = 0; y < height && !hasWallChar && !hasFloorChar; y++)
			for (int x = 0; x < width && !hasWallChar && !hasFloorChar; x++)
			{
				if (result[y, x] == wallChar)
					hasWallChar = true;
				if (result[y, x] == floorChar)
					hasFloorChar = true;
			}

		hasWallChar.Should().BeFalse();
		hasFloorChar.Should().BeFalse();
	}
}
