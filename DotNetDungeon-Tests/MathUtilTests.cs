namespace DotNetDungeon_Tests;

using DotNetDungeon_Utils;
using FluentAssertions;

[TestClass]
public sealed class MathUtilTests
{
	[TestMethod]
	public void GenerateRandomInteger_MinIsGreaterThanMax()
	{
		// Arrange
		int min = 2;
		int max = 1;

		// Act
		Action act = () => MathUtil.GenerateRandomInteger(min, max);

		// Assert
		act.Should().Throw<Exception>();
	}

	[TestMethod]
	public void GenerateRandomInteger_MinIsEqualToMax()
	{
		// Arrange
		int min = 1;
		int max = 1;

		// Act
		int result = MathUtil.GenerateRandomInteger(min, max);

		// Assert
		result.Should().Be(min);
	}

	[TestMethod]
	public void GenerateRandomInteger_MinIsLessThanMax()
	{
		// Arrange
		int min = 1;
		int max = 2;

		// Act
		int result = MathUtil.GenerateRandomInteger(min, max);

		// Assert
		result.Should().BeGreaterThanOrEqualTo(min);
		result.Should().BeLessThanOrEqualTo(max);
	}
}