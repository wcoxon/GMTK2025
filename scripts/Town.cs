using Godot;
using System.Collections.Generic;

public partial class Town : Node3D
{
    // ref
    MeshInstance3D meshInstance;

    // town resource handle
    private TownData data;
    [Export] TownData Data { get => data; set => data = value; }

    // properties exposing resource info
    public string TownName => data.townName; 
    public float[] Production => data.production; 
    public float[] Consumption => data.consumption; 
    public AudioStream Theme => data.theme; 

    public int Population { get => data.population; set => data.population = value; }
    public int Wealth { get => data.wealth; set => data.wealth = value; }
    public float[] Stocks { get => data.stocks; set => data.stocks = value; }

    public Godot.Collections.Array<Traveller> Visitors = [];
    static RandomNumberGenerator rng = new();
    static Color selectionColour = new(1, 0, 0),  hoverColour = new(1, .5f, .8f);

    private bool selected;
    public bool Selected
    {
        get => selected;
        set
        {
            selected = value;
            meshInstance.SetInstanceShaderParameter("colour", selectionColour);
            meshInstance.SetInstanceShaderParameter("width", selected ? 0.3 : 0);
        }
    }
    public void select() => Player.Instance.SelectedTown = this;
    
    bool Hovered
    {
        set
        {
            if (Selected) return;
            meshInstance.SetInstanceShaderParameter("colour", hoverColour);
            meshInstance.SetInstanceShaderParameter("width", value ? 0.1 : 0);
        }
    }
    public void hover() => Hovered = true;
    public void unhover() => Hovered = false;

    public override void _EnterTree()
    {
        meshInstance = GetNode<MeshInstance3D>("MeshInstance");
        meshInstance.Mesh = data.mesh;
        meshInstance.SetSurfaceOverrideMaterial(0, data.material);

        Player.Instance.World.Towns.Add(this);
    }

    public override void _Ready()
    {

        //for (int i = 0; i < 3; i++) data.prices[i] = Game.itemBaseValues[i] * data.price_multiplyers[i];

        //data.base_consumption.CopyTo(data.consumption, 0);
        //data.base_production.CopyTo(data.production, 0);
    }
    public void onInput(Node cam, InputEvent e, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (e is InputEventMouseButton && Input.IsActionJustPressed("select")) select();
    }
    

    public float netProduction(int itemID) => Production[itemID] - Consumption[itemID];
    public int appraise(int itemID)
    {
        // ok infinite money on towns ig

        // so no need to worry about towns balancing their wealths for now i think

        
        int priceOffset = -Mathf.Sign(netProduction(itemID));

        return Game.itemBaseValues[itemID] + priceOffset;
    }

    public override void _PhysicsProcess(double delta)
    {
        double simDelta = delta * Player.Instance.World.timeScale;
        updateStock(simDelta);
    }

    public bool isFunctioning()
    {
        for (int item = 0; item < 3; item++)
        {
            if (netProduction(item) < 0 && Stocks[item] <= 0) return false;
        }
        return true;
    }
    void updateStock(double simDelta)
    {
        if (!isFunctioning())
        {
            meshInstance.Hide();
            return;
        }
        meshInstance.Show();

        for (int item = 0; item < 3; item++)
        {
            float netProduction = Production[item] - Consumption[item];

            Stocks[item] += netProduction * (float)simDelta / 60; // production per hour or something
            Stocks[item] = Mathf.Max(Stocks[item], 0); // make sure it doesn't go negative
        }

        // i could actually be like if the item goes to 0 set to dysfunctional
        // and on traded with check for making functional again
        // would save the for loop getter
    }
}
