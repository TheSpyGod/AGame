using System;

namespace AGConsole.Engine.Render
{
	class Generation
	{
		public char[,] Map { get; private set; }
		private const int CellSize = 20; // Adjust cell size to fit the window
		private const int MapWidth = 800;
		private const int MapHeight = 600;
		private readonly int widthInCells;
		private readonly int heightInCells;
		private static readonly Random Rand = new Random();

		public Generation()
		{
			widthInCells = MapWidth / CellSize;
			heightInCells = MapHeight / CellSize;
		}

		public char[,] TileSet()
		{
			Map = new char[widthInCells, heightInCells];
			InitializeEmptyMap();

			for (var i = 1; i < widthInCells - 1; i++)
			{
				for (var j = 1; j < heightInCells - 1; j++)
				{
					if (Rand.Next(10) == 0) 	
					{
						Map[i, j] = '*';
					}
				}
			}

			return Map;
		}

		private void InitializeEmptyMap()
		{
			// Set borders to '*' and inner cells to ' '
			for (var i = 0; i < widthInCells; i++)
			{
				for (var j = 0; j < heightInCells; j++)
				{
					if (i == 0 || i == widthInCells - 1 || j == 0 || j == heightInCells - 1)
					{
						Map[i, j] = '*';
					}
					else
					{
						Map[i, j] = ' ';
					}
				}
			}
		}
	}
}