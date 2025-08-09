namespace DotNetDungeon_Objets;

public class RoomObject
{
	public int Y { get; }
	public int X { get; }
	public int Height { get; }
	public int Width { get; }

	/// <summary>
	/// Creates a new room with the specified position and dimensions
	/// </summary>
	/// <param name="y">The y-coordinate of the room's position</param>
	/// <param name="x">The x-coordinate of the room's position</param>
	/// <param name="height">The height of the room</param>
	/// <param name="width">The width of the room</param>
	public RoomObject(int y, int x, int height, int width)
	{
		Y = y;
		X = x;
		Height = height;
		Width = width;
	}
}
