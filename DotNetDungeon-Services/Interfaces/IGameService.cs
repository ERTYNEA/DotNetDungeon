namespace DotNetDungeon_Services.Interfaces;

public interface IGameService
{
	char[,] GenerateDungeon(int height, int width, int minProbability, int maxProbability, char airChar, char wallChar);
}