using Godot;

public partial class EncounterArea : Area3D
{
    [Export] public EncounterData data;
    protected EncounterRumour rumour;

    public override void _EnterTree()
    {
        rumour = new(this);
    }

    public override void _Ready()
    {
        BodyEntered += OnEncounterEntered;
        BodyExited += OnEncounterExited;
    }

    public virtual void OnEncounterEntered(Node3D Body)
    { 
        Node other = Body.GetParent();

        if (other is NPCTraveller npc)
        {
            npc.AddRumour(rumour);
        }
    }
    public virtual void OnEncounterExited(Node3D Body) { }

    public virtual void chooseOption(int index) { }
}
