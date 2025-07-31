using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class PlayerView : Node3D
{
    // this will hand logic for the actual player interfacing stuff like moving the camera
    // selecting town and controlling UI shit too, like updating ui when new town is selected

    //refs to other stuff in the scene
    [Export] PlayerTraveller player; // player could instead assign itself to this on ready?
    [Export] TownPanel townPanel;

    [Export] Waypoints waypoints;

    [Export] ScaleTime pauseButton; // we want to simulate pushing these buttons rather than fucking with the timescale directly, so the UI updates correctly
    [Export] ScaleTime playButton;

    public Vector3 cameraVelocity;
    public const float cameraAcceleration = 9.0f;
    [Export] public float maxSpeed = 6.0f;

    public static PlayerView instance; // this class gonna be a singleton

    public float worldSpeed = 1; // this is for scaling delta time in the world simulation stuff

    public void setWorldSpeed(float timescale)
    {
        // just made this method so the speed buttons signals could connect to this to change speed
        worldSpeed = timescale;
    }

    public void PauseWorldSpeed()
    {
        pauseButton.ButtonPressed = true;

        setWorldSpeed(pauseButton.newTimeScale);
    }

    public void PlayWorldSpeed()
    {
        playButton.ButtonPressed = true;

        setWorldSpeed(playButton.newTimeScale);
    }

    Town selectedTown = null;
    public Town SelectedTown
    {
        get => selectedTown;
        set
        {
            if (selectedTown is not null) selectedTown.Selected = false;
            selectedTown = value;
            selectedTown.Selected = true;

            townPanel.Target = selectedTown;

            //player.Target = selectedTown;
        }
    }

    public override void _Ready()
    {
        waypoints.lastDot = player.lastTown;
        instance = this;
        cameraVelocity = new Vector3(0,0,0);
    }

    // drag mouse to pan camera, should do some easing on it, maybe place a grabber on the surface
    // if the mouse moves whilst pressed use relative motion to shift our XZ
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Right))
        {
            Translate(new Vector3(mouseMotion.Relative.X, 0, mouseMotion.Relative.Y) * -0.05f); // 0.05 being pan sensitivity
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = new Vector3(cameraVelocity.X, 0, cameraVelocity.Z);

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
        
        Translate(cameraVelocity); // 0.05 being pan sensitivity
    }


    public void plotJourney()
    {
        // right now this part is a stub, this would be what initiates the waypoint placing thing
        // but rn you just beeline for the selected town
        // player.Target = SelectedTown;
        waypoints.Active = !waypoints.Active;
        townPanel.Embarkmode = waypoints.Active ? TownPanel.EmbarkMode.Embarking : TownPanel.EmbarkMode.Planning;
        if (waypoints.Active)
        {
            waypoints.endDot = SelectedTown;
            waypoints.OnMouseExited();
        }
        else
        {
            var (nodes, dashes) = waypoints.PopJourney();
            player.SetJourney(nodes, dashes);
        }
    }
}
