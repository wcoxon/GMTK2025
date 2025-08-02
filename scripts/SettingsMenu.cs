using Godot;
using System;

public partial class SettingsMenu : Control
{
    public void _ready() {
        GetNode<HSlider>("SettingsContainer/SoundEffectsSlider").Value = AudioServer.GetBusVolumeLinear(1);
        GetNode<HSlider>("SettingsContainer/MusicSlider").Value = AudioServer.GetBusVolumeLinear(2);
    }

    public void returnToMainMenu() {
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
    
    public void onSoundEffectsSliderChange(float volume) {
        AudioServer.SetBusVolumeLinear(1, volume);
    }

    public void onMusicSliderChange(float volume) {
        AudioServer.SetBusVolumeLinear(2, volume);
    }
}