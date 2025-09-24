using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Waypoints : Node3D
{
    [Export] public PackedScene dot;
    [Export] public WorldMap world;

    public Node3D cursorDot;

    private List<Node3D> dots = [];
    public Journey editingJourney;

    private bool active = false;
    public bool Active
    {
        get => active;
        set { active = value; cursorDot.Visible = active; }
    }

    public override void _Ready()
    {
        AddChild(cursorDot = dot.Instantiate<Node3D>());
        cursorDot.GetNode<Area3D>("Area3D").InputRayPickable = false;
    }

    public void OnMapPressed(Node _cam, InputEvent evt, Vector3 evtPos, Vector3 _normal, int _shapeIndex)
    {
        if (!active) return;

        if (evt is InputEventMouseButton mbEvent && mbEvent.Pressed && mbEvent.ButtonIndex == MouseButton.Left)
        {
            var instance = dot.Instantiate<Node3D>();
            AddChild(instance);

            instance.Position = evtPos;
            instance.Position += Vector3.Up * world.getMapHeight(evtPos); // place on terrain inni
            

            // make it monitorable for mouse 
            Area3D instancearea = instance.GetNode<Area3D>("Area3D");
            //instancearea.InputRayPickable = true; // why not just set cursor thing to not be

            instancearea.Connect(Area3D.SignalName.InputEvent, Callable.From<Camera3D, InputEvent, Vector3, Vector3, int>((a, b, c, d, e) => onDotPressed(a, b, c, d, e, instance)));
            // connect its input event to delete on right click function

            dots.Add(instance);
            editingJourney.path.Curve.AddPoint(instance.Position,null,null,dots.Count);
        }

        cursorDot.Visible = true;
        cursorDot.Position = evtPos;
        cursorDot.Position += Vector3.Up * world.getMapHeight(evtPos);
        
    }

    public void onDotPressed(Camera3D _cam, InputEvent @event, Vector3 evtPos, Vector3 _normal, int _shapeIndex, Node3D instance)
    {
        // remove waypoint
        if (!Input.IsActionJustPressed("drag")) return;

        int wayPointIndex = dots.IndexOf(instance);
        dots.RemoveAt(wayPointIndex);
        editingJourney.path.Curve.RemovePoint(wayPointIndex+1); // delete path point corresponding to this dot (dot index + starting point in path)
        instance.QueueFree(); // delete right clicked dot

    }

    public void OnMouseExited() => cursorDot.Visible = false;
    

    public void ClearDots()
    {
        foreach (Node3D dot in dots)
        {
            dot.QueueFree();
        }
        dots = [];
    }
}
