using Godot;
using System;

public partial class SettingsMenu : Control
{
    public void returnToMainMenu() {
        GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
    }
    
    public void onSoundEffectsSliderChange() {
        // TODO: set sound level
    }

    public void onMusicSliderChange() {
        // TODO: set sound level
    }
}