namespace DotNetDungeon_Objets;

public class RoomObject
{
	public int Y { get; }
	public int X { get; }
	public int Height { get; }
	public int Width { get; }

	public RoomObject(int y, int x, int height, int width)
	{
		Y = y;
		X = x;
		Height = height;
		Width = width;
	}
}