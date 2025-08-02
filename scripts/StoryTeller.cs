using Godot;
using System;
using System.Collections.Generic;

public partial class StoryTeller : Node3D
{
    public class ActiveEncounter
    {
        public EncounterContent content;
        public Node3D handler;
        public List<EncounterRumorContent> rumors;

        public double duration;
    }
    public List<ActiveEncounter> ActiveEncounters { get; private set; } = [];

    public BanditEncounter ActiveBandits { get; private set; }

    public StormEncounter ActiveStorm { get; private set; }

    static RandomNumberGenerator rng = new RandomNumberGenerator();

    [Export] PackedScene bandit_encounter;
    [Export] PackedScene storm_encounter;

    void EndBandits()
    {
        ActiveEncounters.Remove(ActiveEncounters.Find(enc => enc.handler == ActiveBandits));
        ActiveBandits.QueueFree();
        ActiveBandits = null;
        GD.Print("Stopped bandits encounter");
    }

    void EndStorm()
    {
        ActiveEncounters.Remove(ActiveEncounters.Find(enc => enc.handler == ActiveStorm));
        ActiveStorm.QueueFree();
        ActiveStorm = null;
        GD.Print("Stopped storm encounter");
    }

    void StartBandits()
    {
        var contents = EncounterManager.Instance.encounters.FindAll(enc => enc.Class == "bandits");
        var content = contents[rng.RandiRange(0, contents.Count - 1)];
        GD.Print("Started bandits encounter: " + content.Name);
        ActiveBandits = bandit_encounter.Instantiate<BanditEncounter>();
        ActiveBandits.content = content;
        ActiveEncounters.Add(new ActiveEncounter
        {
            content = content,
            handler = ActiveBandits,
            rumors = [.. content.Rumors],
            duration = content.Duration
        });
        AddChild(ActiveBandits);
    }

    public override void _Ready()
    {
        StartBandits();
    }

    public EncounterRumorContent GetRumor(Town town)
    {
        List<EncounterRumorContent> rumors = [];
        foreach (var enc in ActiveEncounters)
        {
            foreach (var rumor in enc.rumors)
            {
                if (rumor.Location == town.TownName || rumor.Location == "")
                {
                    enc.rumors.Remove(rumor); // Dont display rumor twice.
                    return rumor;
                }
            }
        }
        return null;
    }

    public override void _Process(double delta)
    {
        foreach (var enc in ActiveEncounters)
        {
            // TODO: Fix to use days.
            enc.duration -= delta;
            if (enc.duration < 0)
            {
                if (enc.handler == ActiveBandits)
                    EndBandits();
                else if (enc.handler == ActiveStorm)
                    EndStorm();
                else
                    ActiveEncounters.Remove(enc);
                break;
            }
        }
    }
}
