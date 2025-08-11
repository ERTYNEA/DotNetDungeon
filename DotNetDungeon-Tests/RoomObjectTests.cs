namespace DotNetDungeon_Tests;

using DotNetDungeon_Objets;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public sealed class RoomObjectTests
{
    [TestMethod]
    public void Constructor_Initializes()
    {
        // Arrange
        int y = 10;
        int x = 20;
        int height = 10;
        int width = 5;
        
        // Act
        var room = new RoomObject(y, x, height, width);
        
        // Assert
        room.Y.Should().Be(y);
        room.X.Should().Be(x);
        room.Height.Should().Be(height);
        room.Width.Should().Be(width);
    }
    
    [TestMethod]
    public void Properties_AreReadOnly()
    {
        // Arrange
        int y = 10;
        int x = 20;
        int height = 10;
        int width = 5;
        
        // Act
        var room = new RoomObject(y, x, height, width);
        var yProperty = typeof(RoomObject).GetProperty("Y");
        var xProperty = typeof(RoomObject).GetProperty("X");
        var heightProperty = typeof(RoomObject).GetProperty("Height");
        var widthProperty = typeof(RoomObject).GetProperty("Width");
        
        // Assert
        yProperty.Should().NotBeNull();
        xProperty.Should().NotBeNull();
        heightProperty.Should().NotBeNull();
        widthProperty.Should().NotBeNull();
        
        // Assert
        yProperty!.CanWrite.Should().BeFalse();
        xProperty!.CanWrite.Should().BeFalse();
        heightProperty!.CanWrite.Should().BeFalse();
        widthProperty!.CanWrite.Should().BeFalse();
    }
}
