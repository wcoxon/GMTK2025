using Godot;
using System;

public partial class MovingEncounter : TickBasedEncounter
{
    [Export] public float m_moveSpeed = 1;

    [Export(PropertyHint.Range, "0,100")] public int maxDistanceBeforeRespawn = 10;

    protected Vector3 Displacement { get; set; }


    public override void _Ready()
    {
        base._Ready();

        GenerateRandomDisplacementVector();
    }

    public override void _Process(double delta)
    {
        MoveArea((float)delta);
    }

    public void MoveArea(float delta)
    {
        float dist = m_moveSpeed * delta * (float)Player.Instance.World.timeScale;

        Translate(Displacement.Normalized() * dist);
    }


    protected void GenerateRandomDisplacementVector()
    {
        float xValue = (float)GD.RandRange(-1.0, 1.0);

        float zValue = (float)GD.RandRange(-1.0, 1.0);

        Displacement = new Vector3(xValue, 0, zValue);
    }

    protected bool CheckMaxDistance(Vector3 positionFrom)
    {
        float distanceToVec = Position.DistanceTo(positionFrom);

        GD.Print("Distance Check: " + distanceToVec);

        if (distanceToVec >= maxDistanceBeforeRespawn)
        {
            return true;
        }

        return false;
    }
    
}
