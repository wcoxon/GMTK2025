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

    public List<Traveller> currentTravellers = [];
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
    public void select() => PlayerView.Instance.SelectedTown = this;
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
    }

    public override void _Ready()
    {
        PlayerView.Instance.allTowns.Add(this);
        RumorView.towns.Add(data.townName, this);
        //is this necessary ^

        for (int i = 0; i < 3; i++) data.prices[i] = PlayerView.Instance.itemBaseValues[i] * data.price_multiplyers[i];

        data.base_consumption.CopyTo(data.consumption, 0);
        data.base_production.CopyTo(data.production, 0);
    }

    public void onInput(Node cam, InputEvent e, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (e is InputEventMouseButton && Input.IsActionJustPressed("select")) select();
    }

    public int appraise(Item item) => (int)data.prices[(int)item];
    
    //ngl i don't like this at all sry
    const double UPDATE_INTERVAL = 8 * 3; // Update every 8 hours. // assumes an hour is 3 seconds long without checking the source of that truth
    private double next_update = UPDATE_INTERVAL;
    public override void _PhysicsProcess(double delta)
    {
        next_update -= delta * PlayerView.Instance.worldSpeed;
        if (next_update > 0) return;

        updateStock();
        next_update += UPDATE_INTERVAL;
    }
    
    static float priceByDemand(float base_price, float demand, float base_demand, float demand_elasticity) => base_price * Mathf.Pow(demand / base_demand, 1 / demand_elasticity);
    static float supplyByPrice(float base_supply, float price, float base_price, float supply_elasticity) => base_supply * Mathf.Pow(price/base_price, supply_elasticity);
    void updateStock()
    {
        for (int item = 0; item < 3; item++)
        {
            Production[item] = supplyByPrice(data.base_production[item], data.prices[item], PlayerView.Instance.itemBaseValues[item] * data.price_multiplyers[item], data.production_elasticity);
            Consumption[item] = data.stock_selloff[item] * data.stocks[item] + data.supply_selloff[item] * data.production[item];

            data.prices[item] = priceByDemand(PlayerView.Instance.itemBaseValues[item] * data.price_multiplyers[item], data.consumption[item], data.base_consumption[item], data.consumption_elasticity);
            // wait shouldn't the prices be entirely deterministic though ?

            Stocks[item] += Mathf.Max(-data.stocks[item], (data.production[item] - rng.Randfn(data.consumption[item], data.consumption[item] / 10)) / 3);
        }
    }
}
