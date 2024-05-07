using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class MapGen : TileMap
{
	const int MainLayer = 0;
	const int MainAtlasID = 0;
	const int MainTerrainSet = 0;

	//Initial stack of Green Tiles and Brown Tiles
	int[] brownTiles = Enumerable.Range(16, 10).ToArray();
	int[] greenTiles = Enumerable.Range(2, 14).ToArray();
	int tileStackState = 2; //2 is green, 1 is brown, 0 is empty
	Stack<int> tileStack;
	public override void _Ready()
	{
		GD.Randomize();
		Random rSeed = new Random();
		//greenTiles.Shuffle(); // Commented out because Godot Arrays ARE ASS
		//brownTiles.Shuffle();
		shuffleArray(rSeed, greenTiles);
		tileStack = new Stack<int>(greenTiles);
	}

	// Don't know where to place this as can reuse for many things
	// Shuffle array function based on Fisher-Yate algorithm
	public static void shuffleArray<T>(Random rSeed, T[] origArray)
	{
		//Step 1: For each unshuffled item in the collection
		for (int n = origArray.Count() - 1; n > 0; --n)
		{
			//Step 2: Randomly pick an item which has not been shuffled
			int k = rSeed.Next(n + 1);

			//Step 3: Swap the selected item with the last "unstruck" letter in the collection
			var temp = origArray[n];
			origArray[n] = origArray[k];
			origArray[k] = temp;
		}
	}
	// Copied Modulus function from online as C# % is remainder, not Modulus.
	// Implements a MOD b
	public static float Mod(float a, float b)
	{
		float c = a % b;
		if ((c < 0 && b > 0) || (c > 0 && b < 0))
		{
			c += b;
		}
		return c;
	}

	// On left click on Map tile, generate the 2x3x2 pattern of next random set of Map tiles 
	public void generateTile(Vector2I currentAtlasCoords, Vector2I posClicked)
	{
		if (currentAtlasCoords is (-1, -1)) // No tile from atlas exists here
		{
			// Cant put tiles behind inital tiles (posClicked.X > 0), can't put tiles above upper bound (posClicked.Y > (-3 * (posClicked.X + 1) - 1)),
			// can't put tiles below lower bound (posClicked.X < (int)Math.Ceiling(-1.5 * posClicked.Y) + 2))
			if ((this.tileStackState != 0) & (posClicked.X > 0) & (posClicked.Y > (-3 * (posClicked.X + 1) - 1)) & (posClicked.X < (int)Math.Ceiling(-1.5 * posClicked.Y) + 2))
			{
				GD.Print("No Pattern Detected");
				if (this.tileStackState != 0)
				{
					// Take pattern from randomized set from mainlayer of tileset and position at tilePos coords
					var tilePos = this.DetermineMapPlacement(posClicked);
					GD.Print("Add tile at " + tilePos.ToString());
					SetPattern(MainLayer, tilePos, TileSet.GetPattern(this.tileStack.Pop()));

					if (this.tileStack.Count == 0) // No tiles left in stack, rebuild stack with brown tiles
					{
						this.tileStack = new Stack<int>(this.brownTiles);
						this.tileStackState--;
					}
				}

				else
				{
					throw new InvalidOperationException("No tiles available");
				}
			}
		}

		else
		{
			var cellTerrain = GetCellTileData(MainLayer, posClicked).Terrain; // Get terrain of tile
			GD.Print("Terrain: " + TileSet.GetTerrainName(MainTerrainSet, cellTerrain));
		}
	}
	// Using math to determine how to position tile. Don't understand why it works, it just does
	private Vector2I DetermineMapPlacement(Vector2I posClicked)
	{
		var caseVal = (int)Mod((posClicked.X - (2 * posClicked.Y)), 7);
		switch (caseVal)
		{
			// Case where position clicked would be a center tile
			case 0:
				{
					return new Vector2I(posClicked.X - 1, posClicked.Y - 1);
				}
			// Case where position clicked is immediately to the right of a center tile
			case 1:
				{
					return new Vector2I(posClicked.X - 2, posClicked.Y - 1);
				}
			// Case where position clicked is one of the top two hexes of a center tile
			case 2:
			case 3:
				{
					return new Vector2I(posClicked.X - (caseVal - 1), posClicked.Y);
				}
			// Case where position clicked is one of the bottom two hexes of a center tile
			case 4:
			case 5:
				{
					return new Vector2I(posClicked.X - (caseVal - 4), posClicked.Y - 2);
				}
			// Case (6) where position clicked is immediately to the left of a center tile
			default:
				{
					return new Vector2I(posClicked.X, posClicked.Y - 1);
				}
		}
	}

}
