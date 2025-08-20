using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] PackedScene gameScene;
    public void newGame()
    {
        // transition to mainscene
        GetTree().ChangeSceneToPacked(gameScene);
    }
    public void settings()
    {
        GetTree().ChangeSceneToFile("res://scenes/settings_menu.tscn");
    }
    public void quit()
    {
        GetTree().Quit();
    }
}
