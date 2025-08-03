using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Waypoints : Node3D
{
    [Export] public PackedScene dot;
    [Export] public PackedScene dashes;

    public Node3D curDot;
    [Export] Node3D lastDot;

    Node3D firstDot;
    public Node3D endDot;
    private Dashes curLine;
    private Dashes endLine;

    private List<Node3D> journey_nodes = [];
    private List<Dashes> journey_dashes = [];

    public void SetStart(Node3D point)
    {
        firstDot = lastDot = point;
    }

    private bool active = false;
    public bool Active
    {
        get => active;
        set { active = value; curDot.Visible = active; curLine.Visible = active; endLine.Visible = active; }
    }

    public override void _Ready()
    {
        AddChild(curDot = dot.Instantiate<Node3D>());
        AddChild(curLine = dashes.Instantiate<Dashes>());
        AddChild(endLine = dashes.Instantiate<Dashes>());
        curDot.Visible = false;
    }

    public void OnMapPressed(Node _cam, InputEvent evt, Vector3 evtPos, Vector3 _normal, int _shapeIndex)
    {
        if (!active) return;

        if (evt is InputEventMouseButton mbEvent
                && mbEvent.Pressed
                && mbEvent.DoubleClick
                && mbEvent.ButtonIndex == MouseButton.Left)
        {
            var instance = dot.Instantiate<Node3D>();
            instance.Position = evtPos;
            AddChild(instance);
            // make it monitorable for mouse 
            Area3D instancearea = instance.GetNode<Area3D>("Area3D");
            instancearea.InputRayPickable = true;
            
            instancearea.Connect(Area3D.SignalName.InputEvent, Callable.From<Camera3D,InputEvent,Vector3,Vector3,int>((a,b,c,d,e) => onDotPressed(a,b,c,d,e,instance)));
            // connect its input event to delete on right click function

            if (lastDot != null)
            {
                var lineInstance = dashes.Instantiate<Dashes>();

                AddChild(lineInstance);
                lineInstance.SetLine(lastDot.Position, evtPos);
                journey_dashes.Add(lineInstance);
            }

            lastDot = instance;
            journey_nodes.Add(instance);
        }

        curDot.Visible = true;
        curDot.Position = evtPos;
        if (lastDot != null)
        {
            curLine.Visible = true;
            curLine.SetLine(lastDot.Position, evtPos);
        }
        if (endDot != null)
            endLine.SetLine(evtPos, endDot.Position);
    }

    public void onDotPressed(Camera3D _cam, InputEvent @event, Vector3 evtPos, Vector3 _normal, int _shapeIndex, Node3D instance)
    {
        // remove waypoint
        // remove dash line
        // update other dash line to other waypoint
        if (!Input.IsActionJustPressed("drag")) return;

        int waypointIndex = journey_nodes.IndexOf(instance);

        Vector3 prev = journey_dashes[waypointIndex].LineStart;
        Vector3 next = waypointIndex < journey_nodes.Count - 1 ? journey_dashes[waypointIndex+1].LineEnd : endDot.Position;
        
        journey_dashes[waypointIndex].QueueFree();
        journey_dashes.RemoveAt(waypointIndex);
        journey_nodes.Remove(instance);

        if (waypointIndex < journey_nodes.Count)
            journey_dashes[waypointIndex].SetLine(prev, journey_nodes[waypointIndex].Position);
        else
        {
            lastDot = journey_nodes.Count > 0 ? journey_nodes.Last() : firstDot;
            endLine.SetLine(prev, next);
        }

        instance.QueueFree();

    }

    public void OnMouseExited()
    {
        if (!active) return;
        curDot.Visible = false;
        curLine.Visible = false;
        endLine.SetLine(lastDot.Position, endDot.Position);
    }



    public (List<Node3D>, List<Dashes>) PopJourney()
    {
        journey_nodes.Add(endDot);
        var instance = dashes.Instantiate<Dashes>();
        AddChild(instance);
        instance.SetLine(lastDot.Position, endDot.Position);
        journey_dashes.Add(instance);
        var ret = (journey_nodes, journey_dashes);
        journey_nodes = [];
        journey_dashes = [];
        firstDot = lastDot = endDot;
        return ret;
    }
}
