using Godot;
using System;

public partial class PlayerMovementController : Node3D
{
    Camera3D camera;
    MeshInstance3D cursor;
    Plane dragPlane = new Plane(new Vector3(0, 1, 0), 0);

    public Vector3 targetPosition;// what if i used player position and camera position instead of target and player? well
    // the issue there is just that you need that offset on the camera

    public override void _EnterTree()
    {
        camera = GetNode<Camera3D>("../Camera");
        cursor = GetNode<MeshInstance3D>("../Cursor");
    }

    public override void _Input(InputEvent @event)
    {

        bool dragBegin = @event is InputEventMouseButton && Input.IsActionJustPressed("drag");
        bool dragMove = @event is InputEventMouseMotion && Input.IsActionPressed("drag");

        var mousePos = GetViewport().GetMousePosition();

        Vector3 mousePosition = dragPlane.IntersectsRay(camera.ProjectRayOrigin(mousePos), camera.ProjectRayNormal(mousePos)) ?? Vector3.Zero;

        if (dragBegin)
        {
            cursor.Position = mousePosition;
        }
        if (dragMove)
        {
            var dragDelta = mousePosition - cursor.Position; // displacement from start of drag

            targetPosition -= dragDelta; // displace player opposite to drag
        }
    }

    public override void _Process(double delta)
    {
        Player.Instance.Position = Player.Instance.Position.Lerp(targetPosition, 0.3f);
    }



    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("left", "right"), 0, Input.GetAxis("up", "down"));

        targetPosition += inputDirection.Normalized()*30 * (float)delta;
    }
}
