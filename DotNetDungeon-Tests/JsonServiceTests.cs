namespace DotNetDungeon_Tests;

using DotNetDungeon_Services.Interfaces;
using DotNetDungeon_Services.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

[TestClass]
public sealed class JsonServiceTests
{
	private IJsonService jsonService = null!;
	private string testFilesPath = null!;

	[TestInitialize]
	public void Initialize()
	{
		// Create a new instance of the service
		jsonService = new JsonService();

		// Create a directory for test files
		testFilesPath = Path.Combine(Path.GetTempPath(), "DotNetDungeonTests");
		if (!Directory.Exists(testFilesPath))
			Directory.CreateDirectory(testFilesPath);
	}

	[TestMethod]
	public void ConvertJsonPathToModelObject_FileDoesNotExist()
	{
		// No create a test JSON file
		string jsonFilePath = Path.Combine(testFilesPath, "notest.json");

		// Arrange
		var testModel = new TestModel();

		// Act
		Action act = () => jsonService.ConvertJsonPathToModelObject(jsonFilePath, testModel);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void ConvertJsonPathToModelObject_EmptyFile()
	{
		// Create a test empty JSON file
		string jsonFilePath = Path.Combine(testFilesPath, "emptytest.json");
		File.WriteAllText(jsonFilePath, "");

		// Arrange
		var testModel = new TestModel();

		// Act
		Action act = () => jsonService.ConvertJsonPathToModelObject(jsonFilePath, testModel);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void ConvertJsonPathToModelObject_InvalidJson()
	{
		// Create a test JSON file
		string jsonFilePath = Path.Combine(testFilesPath, "invalidtest.json");
		string json = "{\"Name\":\"Test\",\"Value\":invalid}";
		File.WriteAllText(jsonFilePath, json);

		// Arrange
		var testModel = new TestModel();

		// Act
		Action act = () => jsonService.ConvertJsonPathToModelObject(jsonFilePath, testModel);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void ConvertJsonPathToModelObject_ReturnsDeserializedObject()
	{
		// Create a test JSON file
		string jsonFilePath = Path.Combine(testFilesPath, "test.json");
		string json = "{\"Name\":\"Test\",\"Value\":42}";
		File.WriteAllText(jsonFilePath, json);

		// Arrange
		var testModel = new TestModel();

		// Act
		var result = jsonService.ConvertJsonPathToModelObject(jsonFilePath, testModel);

		// Assert
		result.Should().NotBeNull();
		result.Name.Should().Be("Test");
		result.Value.Should().Be(42);
	}

	private class TestModel
	{
		public string Name { get; set; } = string.Empty;
		public int Value { get; set; }
	}
}
