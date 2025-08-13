using Godot;

[Tool]
[GlobalClass]
public partial class TownData : Resource
{
    [Export] public string townName;
    [Export] public int population, wealth;

    [Export] public Mesh mesh;
    [Export] public Material material;
    [Export] public AudioStream theme;


    [Export] public float[] stocks = new float[3]; // quantities of items
    [Export] public float[] base_production = new float[3]; // items produced per day
    [Export] public float[] base_consumption = new float[3]; // items consumed per day
    [Export] public float[] price_multiplyers = [1.0f, 1.0f, 1.0f];

    [Export] public float[] supply_selloff = [0.9f, 0.8f, 0.9f];
    [Export] public float[] stock_selloff = [0.3f, 0.2f, 0.4f];


    public float[] production = new float[3];
    public float[] consumption = new float[3];
    public float[] prices = new float[3];

    [Export] public float production_elasticity = 2;
    [Export] public float consumption_elasticity = -2;
}
