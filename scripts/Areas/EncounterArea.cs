using Godot;

public partial class EncounterArea : Area3D
{
    [Export] public EncounterData data;

    public override void _Ready()
    {
        BodyEntered += OnEncounterEntered;
        BodyExited += OnEncounterExited;
    }

    public virtual void OnEncounterEntered(Node3D Body) { }
    public virtual void OnEncounterExited(Node3D Body) { }
    
    public virtual void chooseOption(int index) { }
}
