using Godot;
using System;
using System.Collections.Generic;

public partial class WorldMap : Node3D
{
    public DirectionalLight3D Sun;
    public MeshInstance3D Surface;

    public List<Town> Towns = [];

    ShaderMaterial mapMaterial;
    Noise noise;
    Gradient ramp;


    public double timeScale = 1;
    double time = 60*7; // world time, UI shows the time from the world which stores that now, // world should prob also increment time and all that
                 // it just makes more sense, like time shouldn't stop when the UI isn't there you know, the time is part of the simulation not borrowed from something outside of it
    
    // ok let's say, an hour is 60 seconds at normal speed, making a minute 1 second i.e. time is in minutes

    public override void _Process(double delta)
    {
        time += delta * timeScale;
        
        double dayProg = time / 1440;
        // at 0 rotation the sun is like setting, we want it to be midnight at 0 time so offset by a quarter rotation
        Sun.Rotation = Vector3.Right * Mathf.Tau *  ((float)dayProg+0.25f);

    }

    public string getDateTime()
    {

        int minute = (int)time; // minutes is 1 sec, mod 0-60
        int hour = minute / 60; // hours is 60 sec, mod 0-24
        int day = hour / 24; // day is 24 hr, mod 0-30
        int month = day / 30; // month is 30 days, mod 0-12
        int year = month / 12; // year is 12 months

        minute %= 60;
        hour %= 24;
        day %= 30;
        month %= 12;
        
        return $"{hour.ToString("00")}:{minute.ToString("00")}  {day.ToString("00")}/{month.ToString("00")}/{year.ToString("0000")}";
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        Sun = GetNode<DirectionalLight3D>("Sun");
        Surface = GetNode<MeshInstance3D>("SurfacePlane");


        mapMaterial = GetNode<WorldMap>("../WorldMap").Surface.GetSurfaceOverrideMaterial(0) as ShaderMaterial;
        noise = mapMaterial.GetShaderParameter("heightMap").As<NoiseTexture2D>().Noise;
        ramp = mapMaterial.GetShaderParameter("heightMap").As<NoiseTexture2D>().ColorRamp;
    }

    public float getMapHeight(Vector3 worldPos)
    {
        Vector2 UV = new Vector2(worldPos.X, worldPos.Z)/75.0f + Vector2.One * 0.5f;

        UV *= 256; 


        float noiseSample = noise.GetNoise2D(UV.X, UV.Y);
        float textureSample = ramp.Sample(noiseSample*0.5f + 0.5f).R; //*0.5f+0.5f

        return textureSample * 10;
    }
}
