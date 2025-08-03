using Godot;
using System;
using System.Collections.Generic;

public partial class Town : Node3D
{
    // towns have production (item, rate)
    // consumption (item, rate) (could just use negative production)
    // population (scales production and consumption)
    // stock (list of (item, quantity)), price is calculated in getPrice(item) using quantity and consumption of the item


    //refs, replace with selection in _Ready later probab
    [Export] MeshInstance3D mesh;

    bool selected;
    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            mesh.SetInstanceShaderParameter("colour", new Color(1, 0, 0));
            mesh.SetInstanceShaderParameter("width", selected ? 0.3 : 0);
        }
    }

    // scooping town data out of custom resource because then we can save town data onto resource files
    TownData data;
    [Export]
    TownData Data
    {
        get => data;
        set
        {
            GD.Print("well");
            data = value;

            mesh.Mesh = data.mesh;
        }
    }

    public string TownName { get => data.townName; }
    public int Population { get => data.population; set => data.population = value; }
    public int Wealth { get => data.wealth; set => data.wealth = value; }
    public float[] Stocks { get => data.stocks; set => data.stocks = value; }
    public float[] Production { get => data.production; } // per day
    public float[] Consumption { get => data.consumption; } // per day
    public AudioStream Theme { get => data.theme; }

    public override void _Ready()
    {
        // register for rumors.
        RumorView.towns.Add(data.townName, this);
        
    }

    public void _on_area_3d_input_event(Node cam, InputEvent evt, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (evt is InputEventMouseButton mbEvent && Input.IsActionJustPressed("select"))
        {
            select();
        }
    }

    public void hover()
    {
        if (Selected) return;
        mesh.SetInstanceShaderParameter("colour", new Color(1,0.5f,0.8f));
        mesh.SetInstanceShaderParameter("width", 0.1);
    }
    public void unhover()
    {
        if (Selected) return;
        mesh.SetInstanceShaderParameter("width",0);
    }
    public void select() => PlayerView.Instance.SelectedTown = this;
    
    public int appraise(Item item)
    {
        // price based on
        // how much of that item is in stock at this town

        // consumption, which itself is scaled by population

        // that's kind of it? well for now let's say that's it

        // so the consumption determines whether there's a shortage, since it defines the threshold of not enough and more than enough 


        int itemIndex = (int)item;

        float valueScale = 1;

        valueScale *= Consumption[itemIndex] + 1;
        valueScale /= Production[itemIndex] + 1;


        return (int)(valueScale * PlayerView.Instance.itemBaseValues[itemIndex]);
    }

    public override void _PhysicsProcess(double delta)
    {
        updateStock(delta);
    }


    void updateStock(double deltaRealTime)
    {
        // produce or consume items over time, call in physics process
        double deltaSimTime = deltaRealTime * PlayerView.Instance.worldSpeed;

        // calculate how much of a day this equates to

        // if 3 sim seconds is an hour, then a day is 24*3 sim seconds

        double deltaSimDays = deltaSimTime / (24 * 3);

        for (int item = 0; item < 3; item++)
        {
            Stocks[item] += (Production[item] - Consumption[item]) * (float)deltaSimDays;
        }
    }

    
}
