using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public enum GameState
{
    TOWN,
    PLANNING,
    TRAVELLING,
    ENCOUNTERING
}

public partial class PlayerView : Node3D, EncounterManager.IVariableProvider
{
    // this will hand logic for the actual player interfacing stuff like moving the camera
    // selecting town and controlling UI shit too, like updating ui when new town is selected

    public static PlayerView instance; // this class gonna be a singleton

    //refs to other stuff in the scene
    public PlayerTraveller player;
    [Export] TownPanel townPanel;
    [Export] Waypoints waypoints;
    [Export] ScaleTime pauseButton;
    [Export] ScaleTime playButton;
    [Export] ScaleTime fastSpeedButton;
    [Export] ScaleTime turboSpeedButton;
    [Export] Panel timeControlPanel;
    [Export] InventoryUI inventoryUI;
    [Export] TradeUI tradeUI;

    [Export] public RumorView rumorView;
    [Export] public EncounterView encounterView;

    GameState gameState;
    public GameState State
    {
        get => gameState;
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.TOWN:
                    PauseWorldSpeed();
                    timeControlPanel.Hide();

                    if (townPanel.Town is not null) townPanel.updateActions();
                    break;

                case GameState.PLANNING:
                    waypoints.Active = true;
                    townPanel.Embarkmode = TownPanel.EmbarkMode.Planning; // notifies town UI to update to planning state
                    waypoints.endDot = SelectedTown;
                    waypoints.OnMouseExited();
                    break;

                case GameState.TRAVELLING:
                    waypoints.Active = false;
                    townPanel.Embarkmode = TownPanel.EmbarkMode.Embarking; // notifies town UI to update to embarking state
                    var (nodes, dashes) = waypoints.PopJourney();
                    player.SetJourney(nodes, dashes);
                    player.onDeparture();

                    PlayWorldSpeed();
                    timeControlPanel.Show();
                    break;
            }
        }
    }


    public Vector3 cameraVelocity = Vector3.Zero;
    public const float cameraAcceleration = 200.0f;
    [Export] public float maxSpeed = 20.0f;
    public float worldSpeed = 1; // this is for scaling delta time in the world simulation stuff

    public int[] itemBaseValues = [5, 12, 10]; // idfk where elseto put this i just wanna store each items base value somewhere :'(

    public void setWorldSpeed(float timescale) => worldSpeed = timescale;
    public void PauseWorldSpeed() => pauseButton.ButtonPressed = true;
    public void PlayWorldSpeed() => playButton.ButtonPressed = true;
    public void FastForwardWorldSpeed() => fastSpeedButton.ButtonPressed = true;
    public void TurboWorldSpeed() => turboSpeedButton.ButtonPressed = true;

    private double tick;
    private int eightHourTicker;
    private int dayTicker;

    private void OnTick() //increment our other tickers, then set the tick to zero so we can tick again.
    {
        eightHourTicker += 1;
        dayTicker += 1;

        if (eightHourTicker >= 8)
        {
            EmitSignal(SignalName.EightTicks);
        }

        if (dayTicker >= 24)
        {
            EmitSignal(SignalName.TwentyFourTicks);
        }

        tick = 0;
    }

    private void OnEightTicks() //Reset our eight hour tracking variable, so we can check if it hit eight.
    {
        eightHourTicker = 0;
    }

    private void OnDay()
    {
        dayTicker = 0; //Reset our day tracking variable, so we can check if it hit twenty four.
        GD.Print("Day Passed!");
    }

    [Signal] public delegate void TickEventHandler();
    [Signal] public delegate void EightTicksEventHandler();
    [Signal] public delegate void TwentyFourTicksEventHandler();


    //public void OnStateChanged() // ok honestly it would make more sense to make a state machine at this point but I've spent too long not doing that and it feels like a sunk cost // could we put this stuff in the property, like when you set the state it will implicitly update the ui and stuff to reflect this
    //{
    //    switch (gameState)
    //    {
    //        case GameState.TOWN:
    //            timeControlPanel.Hide();
    //            break;
//
    //        case GameState.PLANNING:
//
    //            break;
//
    //        case GameState.TRAVELLING:
    //            timeControlPanel.Show();
    //            break;
    //    }
    //}

    Town selectedTown;
    public Town SelectedTown
    {
        get => selectedTown;
        set
        {
            if (selectedTown is not null) selectedTown.Selected = false;
            else townPanel.Visible = true;

            selectedTown = value;
            selectedTown.Selected = true;

            townPanel.Town = selectedTown;
        }
    }

    

    public override void _Ready()
    {
        instance = this; // global handle
        EncounterManager.Instance.AddProvider(this);
        
        State = GameState.TOWN;
        player = GetNode<PlayerTraveller>("../Map/Traveller");
        waypoints.lastDot = player.Town; // start path at current town

        tick = 0;
        dayTicker = 0;
        eightHourTicker = 0;

        Tick += OnTick;

        EightTicks += OnEightTicks;

        TwentyFourTicks += OnDay;

        PauseWorldSpeed();
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

        if (@event.IsActionPressed("inventory"))
        {
            inventoryUI.Visible = !inventoryUI.Visible;

            if (inventoryUI.Visible) inventoryUI.displayInventory(player);
        }

        switch (gameState)
        {
            case GameState.TOWN:
                break;

            case GameState.PLANNING:
                break;

            case GameState.TRAVELLING:

                if (@event.IsActionPressed("speed0"))
                {
                    if (worldSpeed == 0) PlayWorldSpeed();
                    else PauseWorldSpeed();
                }
                if (@event.IsActionPressed("speed1")) PlayWorldSpeed();
                if (@event.IsActionPressed("speed2")) FastForwardWorldSpeed();
                if (@event.IsActionPressed("speed3")) TurboWorldSpeed();
                break;

            case GameState.ENCOUNTERING:
                break;
        }
    }

    public override void _Process(double delta)
    {
        tick += delta * worldSpeed / 3; // The same calculation is done in date, but we need it here in the singleton for signalling reasons.

        if (tick >= 1)
        {
            EmitSignal(SignalName.Tick);
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = new Vector3(cameraVelocity.X, 0, cameraVelocity.Z); // could we not edit cameraVelocity directly?

        //Vector2 inputVector = Input.GetVector("left", "right", "up", "down");
        Vector3 inputDirection = new Vector3(Input.GetAxis("left","right"), 0, Input.GetAxis("up","down"));

        if (inputDirection != Vector3.Zero)
        {
            //accelerate in input direction
            velocity += inputDirection * cameraAcceleration * (float)delta;

            //limit speed
            velocity = velocity.Normalized() * Mathf.Min(velocity.Length(), maxSpeed);
        }
        else
        {
            //decelerate towards 0
            velocity = velocity.Normalized() * Mathf.Max(velocity.Length() - cameraAcceleration * (float)delta, 0);
        }

        cameraVelocity = velocity;

        Translate(cameraVelocity * (float)delta);
    }

    public void plotJourney()
    {
        //waypoints.Active = true;
        //townPanel.Embarkmode = TownPanel.EmbarkMode.Planning;
        State = GameState.PLANNING;

        //waypoints.endDot = SelectedTown;
        //waypoints.OnMouseExited(); // (since the mouse is off the map at this moment)
    }

    public void embark()
    {
        State = GameState.TRAVELLING;
        //waypoints.Active = false;
        //townPanel.Embarkmode = TownPanel.EmbarkMode.Embarking;
        //
        //var (nodes, dashes) = waypoints.PopJourney();
        //player.SetJourney(nodes, dashes);
        //
        //player.onDeparture();
    }


    public void openTrade()
    {
        // make trade ui visible
        // populate trade ui with town information
        // trade ui should also handle the button press stuff i think
        
        tradeUI.Town = selectedTown; // i know you should only be able to trade with the town you're on it's just weird that it offers trade in the town panel for whatever you have selected :p
        tradeUI.Visible = true;

    }

    public string VariablesPrefix()
    {
        return "player";
    }

    public List<(string, double)> GetVariables()
    {
        return [
            ("health", 5),
            ("money", 100)
        ];
    }

    public void UpdateVariable(string name, double value)
    {
        if (name == "health") GD.Print("Set playerhealth to " + value);
        else if (name == "money") GD.Print("Set money to " + value);
        else throw new KeyNotFoundException("Player cannot assign to variable " + name);
    }
}
