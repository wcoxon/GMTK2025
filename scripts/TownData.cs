using Godot;
using System;
using System.Collections.Generic;

public partial class TownData : Resource
{
    [Export] public string townName;
    [Export] public int population, wealth;

    [Export] public Mesh mesh;
    [Export] public AudioStream theme;

    
    [Export] public float[] stocks = new float[3]; // quantities of items
    [Export] public float[] production = new float[3]; // items produced per day
    [Export] public float[] consumption = new float[3]; // items consumed per day
}
