using Godot;
using System;

public partial class playerAssigner : Node3D
{
    PlayerView player;
    [Export]
    PlayerView Player
    {
        get => player;
        set => PlayerView.Instance = value;
    }
    public override void _Ready()
    {
        PlayerView.Instance = GetNode<PlayerView>("Player");
    }

}
