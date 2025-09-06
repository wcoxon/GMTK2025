using Godot;
using System;

public partial class Player : Node3D
{
    public static Player Instance;


    [Export] public UIController UI;
    public PlayerTraveller traveller;
    public WorldMap World;

    Waypoints waypoints;
    PlayerMovementController movement;
    MusicController music;


    private PlayerState state;
    public PlayerState State
    {
        get => state;
        set
        {
            state = value;

            switch (state)
            {
                case PlayerState.TOWN:
                    moveTo(traveller.Position); // focus on player
                    music.Track = traveller.Town.Theme; // play town theme tune

                    waypoints.ClearDots(); // clear waypoints

                    UI.timeControlPanel.Pause(); // pause automatically so the player isn't accidentally idling with time passing
                    break;

                case PlayerState.ENCOUNTER:
                    UI.timeControlPanel.Pause();
                    UI.timeControlPanel.disable();
                    break;

                case PlayerState.PLAN:
                    break;

                case PlayerState.TRAVEL:
                    music.Track = null; // play travelling music

                    traveller.onDeparture();

                    UI.timeControlPanel.enable();
                    UI.timeControlPanel.Speed1();
                    break;
            }
        }
    }

    private Town selectedTown;
    public Town SelectedTown
    {
        get => selectedTown;
        set
        {
            if (selectedTown is not null) selectedTown.Selected = false; // deselect last town
            
            selectedTown = value;
            selectedTown.Selected = true; // select town

            UI.townContextMenu.Open(selectedTown);
        }
    }

    public override void _EnterTree()
    {
        Instance = this;
        World = GetNode<WorldMap>("../WorldMap");
        traveller = World.GetNode<PlayerTraveller>("PlayerTraveller");
        waypoints = GetNode<Waypoints>("../Waypoints");
        music = GetNode<MusicController>("MusicPlayer");
        movement = GetNode<PlayerMovementController>("PlayerMovementController");
    }

    public override void _Ready()
    {
        State = PlayerState.TOWN;
    }

    public override void _Input(InputEvent @event)
    {
        Scale += Vector3.One * Input.GetAxis("zoomIn", "zoomOut") * 0.1f;


        //if (@event is not InputEventAction) return;

        if (@event.IsActionPressed("speed0"))
        {
            if (World.timeScale == 0) UI.timeControlPanel.Speed1();
            else UI.timeControlPanel.Pause();
        }
        if (@event.IsActionPressed("speed1")) UI.timeControlPanel.Speed1();
        if (@event.IsActionPressed("speed2")) UI.timeControlPanel.Speed2();
        if (@event.IsActionPressed("speed3")) UI.timeControlPanel.Speed4();
        if (Input.IsKeyPressed(Key.Key4)) UI.timeControlPanel.Speed16();
        if (Input.IsKeyPressed(Key.Key5)) UI.timeControlPanel.Speed64();
    }

    public void plotJourney() {
        UI.plottingUI.Open();
        waypoints.Active = true;
        waypoints.editingJourney = traveller.journey;
        traveller.journey.initJourney(traveller.Town, selectedTown);
        State = PlayerState.PLAN;
    } 
    public void embark()
    {
        waypoints.Active = false; // deactivate waypoints
        State = PlayerState.TRAVEL;
    }
    public void cancelPlot()
    {
        waypoints.Active = false; // deactivate waypoints
        traveller.journey.clearJourney(); // clear path
        waypoints.ClearDots(); // remove placed waypoints
        State = PlayerState.TOWN;
    }

    public void moveTo(Vector3 pos) => movement.targetPosition = pos;
    
    public void OnDeath()
    {
        traveller.Health = 3;
        traveller.Money = Math.Min(50, traveller.Money / 3);
        traveller.Position = traveller.Town.Position;
        State = PlayerState.TOWN;
    }
}
