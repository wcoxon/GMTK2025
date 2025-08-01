using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public enum GameState {
    TOWN,
    PLANNING,
    TRAVELLING

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

    public GameState gameState;


    public Vector3 cameraVelocity = Vector3.Zero;
    public const float cameraAcceleration = 20.0f;
    [Export] public float maxSpeed = 6.0f;
    public float worldSpeed = 1; // this is for scaling delta time in the world simulation stuff

    public void setWorldSpeed(float timescale) => worldSpeed = timescale;
    public void PauseWorldSpeed() => pauseButton.ButtonPressed = true;
    public void PlayWorldSpeed() => playButton.ButtonPressed = true;
    public void FastForwardWorldSpeed() => fastSpeedButton.ButtonPressed = true;
    public void TurboWorldSpeed() => turboSpeedButton.ButtonPressed = true;

    public void ChangeState(GameState newState)
    {
        gameState = newState;

        OnStateChanged();
    }

    public void OnStateChanged() // ok honestly it would make more sense to make a state machine at this point but I've spent too long not doing that and it feels like a sunk cost
    {
        switch (gameState) {
            case GameState.TOWN:
                timeControlPanel.Hide();
            break;

        case GameState.PLANNING:

            break;

        case GameState.TRAVELLING:
                timeControlPanel.Show();
            break;
        }       
    }

    Town selectedTown;
    public Town SelectedTown
    {
        get => selectedTown;
        set
        {
            if (selectedTown is not null) selectedTown.Selected = false;

            selectedTown = value;
            selectedTown.Selected = true;

            townPanel.Target = selectedTown;
        }
    }

    public override void _Ready()
    {
        cameraVelocity = new Vector3(0, 0, 0);

        ChangeState(GameState.TOWN);
        instance = this; // global handle
        player = GetNode<PlayerTraveller>("../Map/Traveller");
        waypoints.lastDot = player.Town; // start path at current town
        EncounterManager.Instance.AddProvider(this);
        EncounterManager.Instance.ApplyEffects("player_health += player_money");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Right))
        {
            // drag mouse to pan camera, should do some easing on it, maybe place a grabber on the surface // if the mouse moves whilst pressed use relative motion to shift our XZ
            float dragScale = 0.05f;
            Vector3 mouseDelta = mouseMotion.Relative.X*Vector3.Left + mouseMotion.Relative.Y*Vector3.Forward; // i should actually base this on camera direction kinda
            
            Translate(mouseDelta * dragScale);
        }

        switch (gameState)
        {
            case GameState.TOWN:

                break;

            case GameState.PLANNING:

                break;

            case GameState.TRAVELLING:

                if (@event.IsActionPressed("speed0")){
                    if (worldSpeed == 0) PlayWorldSpeed();
                    else PauseWorldSpeed();
                }        
                if (@event.IsActionPressed("speed1")) PlayWorldSpeed();
                if (@event.IsActionPressed("speed2")) FastForwardWorldSpeed();
                if (@event.IsActionPressed("speed3")) TurboWorldSpeed();
                break;
        }
        


    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = new Vector3(cameraVelocity.X, 0, cameraVelocity.Z); // could we not edit cameraVelocity directly?

        Vector2 inputVector = Input.GetVector("left", "right", "up", "down");
        Vector3 inputDirection = new Vector3(inputVector.X, 0, inputVector.Y);

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
        waypoints.Active = true;
        townPanel.Embarkmode = TownPanel.EmbarkMode.Planning; 
        
        waypoints.endDot = SelectedTown;
        waypoints.OnMouseExited(); // (since the mouse is off the map at this moment)
    }

    public void embark()
    {
        waypoints.Active = false;
        townPanel.Embarkmode = TownPanel.EmbarkMode.Embarking;

        var (nodes, dashes) = waypoints.PopJourney();
        player.SetJourney(nodes, dashes);

        player.onDeparture();
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
