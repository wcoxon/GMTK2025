using Godot;
using System;

public partial class TickBasedEncounter : EncounterArea
{

    public override void _Ready()
    {
        base._Ready();
        Player.Instance.Tick += DoOnTick;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Player.Instance.Tick -= DoOnTick;
    }


    public virtual void DoOnTick()
    {

    }

}
