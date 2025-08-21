using Godot;
using System;
using System.Collections.Generic;

public enum GameState{ TOWN, PLAN, TRAVEL, ENCOUNTER }
public enum Item { BROTH, PLASTICS, EVIL_WATER }
public partial class Player : Node3D
{
    public int[] itemBaseValues = [5, 12, 10]; // idfk where elseto put this i just wanna store each items base value somewhere :'(

    public static Player Instance;

    //refs to other stuff in the scene
    public PlayerTraveller traveller;

    [Export] Waypoints waypoints;

    //  UI ref
    [Export] public UIController UI;

    public PlayerMovementController MovementController;

    //  audio ref
    [Export] AudioStream[] travelPlaylist;
    AudioStreamPlayer musicPlayer;
    int trackIndex;
    public AudioStream Music
    {
        set
        {
            musicPlayer.Stream = value ?? travelPlaylist[trackIndex++ % travelPlaylist.Length]; // play given track or play next travel song if null
            musicPlayer.Play();
        }
    }

    [ExportGroup("Encounter")]
    //  rumour refs
    [Export] public RumorView rumorView;
    [Export] public EncounterView encounterView;
    [Export] public NotificationManager notificationManager;

    private GameState state;
    public GameState State
    {
        get => state;
        set
        {
            //var oldGameState = gameState;
            state = value;
            switch (state)
            {
                case GameState.TOWN:
                    moveTo(traveller.Position); // focus on player
                    Music = traveller.Town.Theme;

                    waypoints.ClearDots();

                    UI.townPanel.embark();
                    UI.timeControlPanel.Pause();
                    break;

                case GameState.ENCOUNTER:
                    UI.timeControlPanel.Pause();
                    UI.timeControlPanel.disable();
                    break;

                case GameState.PLAN:
                    UI.townPanel.plan();

                    waypoints.Active = true;
                    waypoints.editingJourney = traveller.journey;
                    traveller.journey.initJourney(traveller.Town, selectedTown);
                    break;

                case GameState.TRAVEL:
                    Music = null;
                    UI.timeControlPanel.enable();

                    traveller.onDeparture();

                    UI.townPanel.embark();
                    UI.timeControlPanel.Speed1();
                    break;
            }
        }
    }

    private double tick;
    private int eightHourTicker, dayTicker;
    public int currentDate; // we have hour in the timecontroller thing but wtv im moving all this logic anw

    private void OnTick() //increment our other tickers, then set the tick to zero so we can tick again.
    {
        if (eightHourTicker++ >= 8) EmitSignal(SignalName.EightTicks);
        if (dayTicker++ >= 24) EmitSignal(SignalName.TwentyFourTicks);
        
        tick = 0;
    }

    private void OnEightTicks() => eightHourTicker = 0; //Reset our eight hour tracking variable, so we can check if it hit eight.

    [Signal] public delegate void TickEventHandler();
    [Signal] public delegate void EightTicksEventHandler();
    [Signal] public delegate void TwentyFourTicksEventHandler();
    private void OnDay()
    {
        dayTicker = 0; //Reset our day tracking variable, so we can check if it hit twenty four.
        currentDate += 1;
    }

    private Town selectedTown;
    public Town SelectedTown
    {
        get => selectedTown;
        set
        {
            if (selectedTown is not null) selectedTown.Selected = false;
            else UI.townPanel.Visible = true;

            selectedTown = value;
            selectedTown.Selected = true;

            UI.townPanel.Town = selectedTown;
        }
    }
    
    public WorldMap World;

    public override void _EnterTree()
    {
        Instance = this;

        World = GetNode<WorldMap>("../WorldMap");
        MovementController = GetNode<PlayerMovementController>("PlayerMovementController");
        traveller = World.GetNode<PlayerTraveller>("PlayerTraveller");
        musicPlayer = GetNode<AudioStreamPlayer>("Camera/MusicPlayer");
    }

    public override void _Ready()
    {
        State = GameState.TOWN;
        
        tick = 0;
        dayTicker = 0;
        eightHourTicker = 0;
        currentDate = 0;

        Tick += OnTick;
        EightTicks += OnEightTicks;
        TwentyFourTicks += OnDay;

        UI.timeControlPanel.Pause();
    }

    public override void _Input(InputEvent @event)
    {
        Scale += Vector3.One * Input.GetAxis("zoomIn", "zoomOut") * 0.1f;

        if (@event.IsActionPressed("inventory")) UI.ToggleInventory(traveller);

        switch (State)
        {
            case GameState.ENCOUNTER: // no time controls when in an encounter
                break;

            default:
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
                break;
        }
    }

    public override void _Process(double delta)
    {
        tick += delta * World.timeScale / 3; // could we have more continuous sim rather than large intervals
        if (tick >= 1) EmitSignal(SignalName.Tick);
    }

    public void plotJourney() => State = GameState.PLAN;
    public void embark()
    {
        waypoints.Active = false;
        State = GameState.TRAVEL;
    }
    public void cancelPlot()
    {
        waypoints.Active = false;
        traveller.journey.clearJourney();
        waypoints.ClearDots();

        State = GameState.TOWN; // honestly transitioning into town state should implicitly dispose of waypoint stuff perhaps
    }

    public void moveTo(Vector3 pos)
    {
        MovementController.targetPosition = pos;
    }
    public void OnDeath()
    {
        traveller.Health = 3;
        traveller.Money = Math.Min(50, traveller.Money / 3);
        notificationManager.AddNotification("You passed out! Returning back to last town.");

        State = GameState.TOWN;
        traveller.Position = traveller.Town.Position;

    }
}
