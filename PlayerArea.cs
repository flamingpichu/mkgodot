using Godot;
using System;

public partial class PlayerArea : Node2D
{	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnCurrentTurn(long characterId) {
		//GD.Print("start turn:" +characterId);
		if (characterId == GameSettings.PlayerCharacter) {
			//GD.Print("Disabled: " + GetNode<Button>("EndTurnButton").Disabled.ToString());
			GetNode<Button>("EndTurnButton").Disabled = false;
			//GD.Print("Disabled: " + GetNode<Button>("EndTurnButton").Disabled.ToString());
			GD.Print("It's your turn!");
		}
	}
}
