using Godot;
using System;
using System.Diagnostics;

public partial class PlayerView : Node3D
{
    // this will hand logic for the actual player interfacing stuff like moving the camera
    // selecting town and controlling UI shit too, like updating ui when new town is selected

    //refs to other stuff in the scene
    [Export] PlayerTraveller player; // player could instead assign itself to this on ready?
    [Export] TownPanel townPanel;


    public static PlayerView instance; // this class gonna be a singleton

    public float worldSpeed = 1; // this is for scaling delta time in the world simulation stuff

    public void setWorldSpeed(float timescale)
    {
        // just made this method so the speed buttons signals could connect to this to change speed
        worldSpeed = timescale;
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
        instance = this;
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

    public void plotJourney()
    {
        // right now this part is a stub, this would be what initiates the waypoint placing thing
        // but rn you just beeline for the selected town
        player.Target = SelectedTown;
    }
}
