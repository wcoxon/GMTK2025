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

        if (evt is InputEventMouseButton mbEvent && mbEvent.Pressed && mbEvent.ButtonIndex == MouseButton.Left)
        {
            var instance = dot.Instantiate<Node3D>();
            instance.Position = evtPos;
            instance.Position += Vector3.Up * getMapHeight(evtPos);
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
                lineInstance.SetLine(lastDot.Position, instance.Position);
                journey_dashes.Add(lineInstance);
            }

            lastDot = instance;
            journey_nodes.Add(instance);
        }

        curDot.Visible = true;
        curDot.Position = evtPos;
        curDot.Position += Vector3.Up * getMapHeight(evtPos);
        

        if (lastDot != null)
        {
            curLine.Visible = true;
            curLine.SetLine(lastDot.Position, evtPos);
        }
        if (endDot != null)
            endLine.SetLine(evtPos, endDot.Position);
    }

    float getMapHeight(Vector3 worldPos)
    {
        ShaderMaterial mapMaterial = GetNode<WorldMap>("../WorldMap").Surface.GetSurfaceOverrideMaterial(0) as ShaderMaterial;
        Noise noise = mapMaterial.GetShaderParameter("heightMap").As<NoiseTexture2D>().Noise;
        Gradient ramp = mapMaterial.GetShaderParameter("heightMap").As<NoiseTexture2D>().ColorRamp;

        //ok so turn world coord into map UV
        // the thing is like 15x15 units, so i guess we just map -7.5 -> 7.5 in world to 0 to 1
        // which means like, divide by 15 and add 0.5 ig
        // shit the texture is normalized too though. also ts aint working

        Vector2 UV = new Vector2(worldPos.X, worldPos.Z)/75.0f + Vector2.One * 0.5f;

        UV *= 256; // turns out we want to sample pixel tho

        // shit it's like, the texture itself was offset by what, 0.5? 

        // i think, uh i thiink that, our ramp is like our own range
        // we go from 0 to 1 from 0.4 to 0.74
        // so when the noise is 0.4 that's 0
        // yeah but we get that from sampling the ramp, 
        // yeah but what do we sample with? idk same noise value as what worked?
        // what even worked
        // ok so just multiplying the sample by 5, when i offset the shader's height sample by -0.5 so that it would be between 5 and -5
        // which shows the raw noise is between 1 and -1, and that the texture is between 0 and 1, because making it -0.5 to 0.5 and *10 
        // matches with making the raw noise *0.5 and *10
        // so if we *0.5 + 0.5 we make our raw noise 0 to 1
        // then sampling the ramp 


        float noiseSample = noise.GetNoise2D(UV.X, UV.Y);
        float textureSample = ramp.Sample(noiseSample*0.5f + 0.5f).R; //*0.5f+0.5f

        return textureSample * 10;
    }

    public void onDotPressed(Camera3D _cam, InputEvent @event, Vector3 evtPos, Vector3 _normal, int _shapeIndex, Node3D instance)
    {
        // remove waypoint
        // remove dash line
        // update other dash line to other waypoint
        if (!Input.IsActionJustPressed("drag")) return;

        int waypointIndex = journey_nodes.IndexOf(instance);

        Vector3 prev = journey_dashes[waypointIndex].LineStart;
        Vector3 next = waypointIndex < journey_nodes.Count - 1 ? journey_dashes[waypointIndex + 1].LineEnd : endDot.Position;

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
