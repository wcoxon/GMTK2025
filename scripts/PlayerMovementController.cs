using Godot;
using System;

public partial class PlayerMovementController : Node3D
{
    Camera3D camera;
    MeshInstance3D cursor;
    Plane dragPlane = new Plane(new Vector3(0, 1, 0), 0);

    public Vector3 targetPosition;

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

        Vector3 dragPosition = dragPlane.IntersectsRay(camera.ProjectRayOrigin(mousePos), camera.ProjectRayNormal(mousePos)) ?? Vector3.Zero;

        if (dragBegin) cursor.Position = dragPosition;

        if (dragMove)
        {
            var dragDelta = dragPosition - cursor.Position; // displacement from cursor noted at start of drag

            targetPosition -= dragDelta; // displace player opposite to drag

            // if we add offset from cursor to target position then whilst lerping its adding a shit tonne more on
        }
    }

    public override void _Process(double delta)
    {
        Player.Instance.Position = targetPosition;
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 inputDirection = new Vector3(Input.GetAxis("left", "right"), 0, Input.GetAxis("up", "down"));

        targetPosition += inputDirection.Normalized()*30 * (float)delta;
    }
}
