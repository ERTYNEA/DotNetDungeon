namespace DotNetDungeon_Tests;

using DotNetDungeon_Objets;
using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spectre.Console;

[TestClass]
public sealed class GameServiceTests
{
	private IGameService gameService = null!;
	private TitleObject nothingTitleObject = null!;
	private TitleObject wallTitleObject = null!;
	private TitleObject floorTitleObject = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Create a new instance of the service
		gameService = new GameService();

		// Initialize TitleObject
		nothingTitleObject = new TitleObject
		{
			CharacterChar = ' ',
			CharacterColorText = Color.Black,
			CharacterColorBackground = Color.Black
		};

		// Initialize TitleObject
		wallTitleObject = new TitleObject
		{
			CharacterChar = '#',
			CharacterColorText = Color.Grey,
			CharacterColorBackground = Color.White
		};

		// Initialize TitleObject
		floorTitleObject = new TitleObject
		{
			CharacterChar = '.',
			CharacterColorText = Color.Grey69,
			CharacterColorBackground = Color.Black
		};
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
			nothingTitleObject,
			wallTitleObject,
			floorTitleObject);

		// Assert
		result.Should().NotBeNull();
		result.GetLength(0).Should().Be(height);
		result.GetLength(1).Should().Be(width);
	}

	[TestMethod]
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
			nothingTitleObject,
			wallTitleObject,
			floorTitleObject);

		// Assert
		bool hasNothingChar = false;
		for (int y = 0; y < height && !hasNothingChar; y++)
			for (int x = 0; x < width && !hasNothingChar; x++)
				if (result[y, x].CharacterChar == nothingTitleObject.CharacterChar)
					hasNothingChar = true;

		hasNothingChar.Should().BeTrue();
	}

	[TestMethod]
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
			nothingTitleObject,
			wallTitleObject,
			floorTitleObject);

		// Assert
		bool hasWallChar = false;
		for (int y = 0; y < height && !hasWallChar; y++)
			for (int x = 0; x < width && !hasWallChar; x++)
				if (result[y, x].CharacterChar == wallTitleObject.CharacterChar)
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
			nothingTitleObject,
			wallTitleObject,
			floorTitleObject);

		// Assert
		bool hasFloorChar = false;
		for (int y = 0; y < height && !hasFloorChar; y++)
			for (int x = 0; x < width && !hasFloorChar; x++)
				if (result[y, x].CharacterChar == floorTitleObject.CharacterChar)
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
			nothingTitleObject,
			wallTitleObject,
			floorTitleObject);

		// Assert
		bool hasWallChar = false;
		bool hasFloorChar = false;
		for (int y = 0; y < height && !hasWallChar && !hasFloorChar; y++)
			for (int x = 0; x < width && !hasWallChar && !hasFloorChar; x++)
			{
				if (result[y, x].CharacterChar == wallTitleObject.CharacterChar)
					hasWallChar = true;
				if (result[y, x].CharacterChar == floorTitleObject.CharacterChar)
					hasFloorChar = true;
			}

		hasWallChar.Should().BeFalse();
		hasFloorChar.Should().BeFalse();
	}
}
