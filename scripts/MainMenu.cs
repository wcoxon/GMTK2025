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
        // transition to settings menu scene (not implemented)
    }
    public void quit()
    {
        GetTree().Quit();
    }
}
