using Godot;
using System;

public partial class MusicController : AudioStreamPlayer
{
    [Export] AudioStream[] travelPlaylist;
    int trackIndex;

    public AudioStream Track
    {
        set
        {
            Stream = value ?? travelPlaylist[trackIndex++ % travelPlaylist.Length]; // play given track or play next travel song if null
            Play();
        }
    }
}
