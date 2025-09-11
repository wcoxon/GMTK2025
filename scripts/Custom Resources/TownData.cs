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

    [Export]public float[] production = new float[3];
    [Export]public float[] consumption = new float[3];
}
