using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Waypoints : Node3D
{
    [Export] public PackedScene dot;

    [Export] public PackedScene dashes;

    public Node3D curDot;
    [Export] public Node3D lastDot;
    public Node3D endDot;
    private Dashes curLine;
    private Dashes endLine;

    private List<Node3D> journey_nodes = [];
    private List<Dashes> journey_dashes = [];

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
    }

    // public void NodeWaypointPreview(Node3D node)
    // {
    // 	curDot.Visible = false;
    // 	if (lastDot != null)
    // 	{
    // 		curLine.SetLine(lastDot.Position, node.Position);
    // 	}
    // }

    // public void NodeWaypoint(Node3D node)
    // {
    // 	if (lastDot != null)
    // 	{
    // 		var lineInstance = dashes.Instantiate<Dashes>();
    // 		AddChild(lineInstance);
    // 		lineInstance.SetLine(lastDot.Position, node.Position);
    // 		lineInstance.SetProgression(0.5);
    // 	}

    // 	lastDot = node;
    // }

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
            curLine.SetLine(lastDot.Position, evtPos);
        if (endDot != null)
            endLine.SetLine(evtPos, endDot.Position);
    }

    public Tuple<List<Node3D>, List<Dashes>> PopJourney()
    {
        journey_nodes.Add(endDot);
        var instance = dashes.Instantiate<Dashes>();
        AddChild(instance);
        instance.SetLine(lastDot.Position, endDot.Position);
        journey_dashes.Add(instance);
        var ret = new Tuple<List<Node3D>, List<Dashes>>(journey_nodes, journey_dashes);
        journey_nodes = [];
        journey_dashes = [];
        lastDot = endDot;
        return ret;
    }
}
