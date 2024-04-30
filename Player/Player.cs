using Godot;
using System;

public partial class Player : Node2D
{
	public override void _Ready()
	{	
		var mapGen = GetNode<TileMap>("../MapGen");
		//GD.Print(mapGen.ToGlobal(mapGen.MapToLocal(new Vector2I(0,0))));
		GlobalPosition = mapGen.ToGlobal(mapGen.MapToLocal(new Vector2I(0,0)));
	}
	public override void _Input(InputEvent @event)
    {
		//if (@event is InputEventMouseButton eventMouseButton)
			//if (eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Left)
			//{
				//var globalClicked = eventMouseButton.Position;
				//var posClicked = LocalToMap(ToLocal(globalClicked));
				//GD.Print("TileMap: " + posClicked.ToString());
				//var currentAtlasCoords = GetCellAtlasCoords(MainLayer, posClicked);
				//GD.Print("Atlas: " + currentAtlasCoords.ToString());
			//}
	}
}
