using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class PlayerView : Node3D
{
    // this will hand logic for the actual player interfacing stuff like moving the camera
    // selecting town and controlling UI shit too, like updating ui when new town is selected

    //refs to other stuff in the scene
    public PlayerTraveller player; // player assigns itself to this on its _ready

    [Export] TownPanel townPanel;
    [Export] Waypoints waypoints;
    [Export] ScaleTime pauseButton; // we want to simulate pushing these buttons rather than fucking with the timescale directly, so the UI updates correctly
    [Export] ScaleTime playButton;

    public Vector3 cameraVelocity = Vector3.Zero;
    public const float cameraAcceleration = 9.0f;
    [Export] public float maxSpeed = 6.0f;

    public static PlayerView instance; // this class gonna be a singleton

    public float worldSpeed = 1; // this is for scaling delta time in the world simulation stuff

    public void setWorldSpeed(float timescale) => worldSpeed = timescale;
    public void PauseWorldSpeed() => pauseButton.ButtonPressed = true;
    public void PlayWorldSpeed() => playButton.ButtonPressed = true;
    

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
        instance = this; // global handle
        player = GetNode<PlayerTraveller>("../Map/Traveller");
        waypoints.lastDot = player.Town; // start path at current town
    }

    // drag mouse to pan camera, should do some easing on it, maybe place a grabber on the surface
    // if the mouse moves whilst pressed use relative motion to shift our XZ
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Right))
        {
            float dragScale = 0.05f;
            Vector3 mouseDelta = mouseMotion.Relative.X*Vector3.Right + mouseMotion.Relative.Y*Vector3.Back; // should actually base this on camera direction kinda
            
            Translate(mouseDelta * dragScale);
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
}
