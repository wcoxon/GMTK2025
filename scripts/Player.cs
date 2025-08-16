using Godot;
using System;
using System.Collections.Generic;

public enum GameState{ TOWN, PLAN, TRAVEL, ENCOUNTER }

public partial class Player : Node3D, EncounterManager.IVariableProvider
{
    public static Player Instance;

    //refs to other stuff in the scene
    public PlayerTraveller traveller;

    [Export] Waypoints waypoints;

    //  UI ref
    [Export] UIController UI;

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
    //  rumour shit refs
    [Export] public RumorView rumorView;
    [Export] public EncounterView encounterView;
    [Export] public NotificationManager notificationManager;


    public int[] itemBaseValues = [5, 12, 10]; // idfk where elseto put this i just wanna store each items base value somewhere :'(
    //public List<Town> allTowns = []; // store this on world map ?

    
    public Vector3 velocity = Vector3.Zero;
    const float acceleration = 200.0f, maxSpeed = 20.0f;

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
                    Position = traveller.Position; // focus on player
                    Music = traveller.Town.Theme;

                    waypoints.ClearDots();

                    UI.townPanel.embark();
                    UI.timeControlPanel.Pause();
                    break;

                case GameState.ENCOUNTER:
                    UI.timeControlPanel.Pause();
                    break;

                case GameState.PLAN:
                    UI.townPanel.plan();

                    waypoints.Active = true;
                    waypoints.editingJourney = traveller.journey;
                    traveller.journey.initJourney(traveller.Town, selectedTown);
                    break;

                case GameState.TRAVEL:
                    Music = null;

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
        //GD.Print("Day Passed!");
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
    public void setWorldSpeed(float speed) => World.timeScale = speed;

    public override void _EnterTree()
    {
        Instance = this;

        World = GetNode<WorldMap>("../WorldMap");
        traveller = World.GetNode<PlayerTraveller>("PlayerTraveller");

        musicPlayer = GetNode<AudioStreamPlayer>("Camera/MusicPlayer");
    }

    public override void _Ready()
    {
        EncounterManager.Instance.AddProvider(this);

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
        if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Right))
        {
            // drag mouse to pan camera, should do some easing on it, maybe place a grabber on the surface // if the mouse moves whilst pressed use relative motion to shift our XZ
            float dragScale = 0.05f;
            Vector3 mouseDelta = mouseMotion.Relative.X * Vector3.Left + mouseMotion.Relative.Y * Vector3.Forward; // i should actually base this on camera direction kinda

            Translate(mouseDelta * dragScale);
        }

        if (@event.IsActionPressed("inventory")) UI.ToggleInventory(traveller);

        if (@event.IsActionPressed("zoomIn")) Scale -= Vector3.One*0.1f;
        if (@event.IsActionPressed("zoomOut")) Scale += Vector3.One*0.1f;
        
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
                break;
        }
    }

    public override void _Process(double delta)
    {
        tick += delta * World.timeScale / 3; // The same calculation is done in date, but we need it here in the singleton for signalling reasons. /// nooo magic number nooo why r u using a literal bro/// maybe it was me that did this, rly bad

        if (tick >= 1) EmitSignal(SignalName.Tick);
    }

    Vector3 clampVector(Vector3 vector, float maxLength)
    {
        if (maxLength < 0) return Vector3.Zero;
        return vector.Normalized() * Mathf.Min(vector.Length(), maxLength);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("left", "right"), 0, Input.GetAxis("up", "down"));

        float deltaSpeed = acceleration * (float)delta; // calculate acceleration/deceleration

        if (!inputDirection.IsZeroApprox())
        {
            velocity += inputDirection * deltaSpeed; // accelerate in input direction
            velocity = clampVector(velocity, maxSpeed); // limit speed
        }
        else
        {
            velocity = clampVector(velocity, velocity.Length() - deltaSpeed); // decelerate towards 0
        }

        Translate(velocity * (float)delta);
    }

    public void plotJourney() => State = GameState.PLAN;
    public void embark()
    {
        waypoints.Active = false;
        traveller.journey.path.Curve.AddPoint(traveller.journey.destination.Position);

        State = GameState.TRAVEL;
    }
    public void cancelPlot()
    {
        State = GameState.TOWN; // honestly transitioning into town state should implicitly dispose of waypoint stuff

        traveller.journey.clearJourney();
        waypoints.Active = false;
        waypoints.ClearDots();
    }

    // idk what ts is about ngl
    public string VariablesPrefix() => "player";
    public List<(string, double)> GetVariables()
    {
        return [
            ("health", traveller.Health),
            ("money", traveller.Money)
        ];
    }
    public void UpdateVariable(string name, double value)
    {
        if (name == "health")
        {
            //GD.Print("Set playerhealth to " + value);
            traveller.Health = (int)value;
        }
        else if (name == "money")
        {
            //GD.Print("Set money to " + value);
            traveller.Money = (int)value;
        }
        else throw new KeyNotFoundException("Player cannot assign to variable " + name);
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
