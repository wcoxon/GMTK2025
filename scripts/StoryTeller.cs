using Godot;
using System;
using System.Collections.Generic;
/*
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

    double next_bandits;
    public BanditEncounter ActiveBandits { get; private set; }

    double next_storm;
    public StormEncounter ActiveStorm { get; private set; }

    static RandomNumberGenerator rng = new RandomNumberGenerator();

    [Export] PackedScene bandit_encounter;
    [Export] PackedScene storm_encounter;

    void EndBandits()
    {
        ActiveEncounters.Remove(ActiveEncounters.Find(enc => enc.handler == ActiveBandits));
        ActiveBandits.QueueFree();
        ActiveBandits = null;
        next_bandits = Math.Clamp(-2 * Math.Log(1 - rng.Randf()), 1, 5);
    }

    void EndStorm()
    {
        ActiveEncounters.Remove(ActiveEncounters.Find(enc => enc.handler == ActiveStorm));
        ActiveStorm.QueueFree();
        ActiveStorm = null;
        next_storm = Math.Clamp(-1.5 * Math.Log(1 - rng.Randf()), 1, 5);
    }

    static EncounterContent RandomEncounter(string cls)
    {
        var contents = EncounterManager.Instance.encounters.FindAll(enc => enc.Class == cls);
        return contents[rng.RandiRange(0, contents.Count - 1)];
    }

    void StartEncounter(EncounterContent content, Node3D handler)
    {
        //GD.Print("Started encounter: " + content.Name);
        ActiveEncounters.Add(new ActiveEncounter
        {
            content = content,
            handler = handler,
            rumors = [.. content.Rumors],
            duration = content.Duration
        });
    }

    //void StartStorm()
    //{
    //    var content = RandomEncounter("storm");
    //    ActiveStorm = storm_encounter.Instantiate<StormEncounter>();
    //    ActiveStorm.content = content;
    //    StartEncounter(content, ActiveStorm);
    //    AddChild(ActiveStorm);
    //}
    //void StartBandits()
    //{
    //    var content = RandomEncounter("bandits");
    //    ActiveBandits = bandit_encounter.Instantiate<BanditEncounter>();
    //    ActiveBandits.content = content;
    //    StartEncounter(content, ActiveBandits);
    //    AddChild(ActiveBandits);
    //}

    //public override void _Ready()
    //{
    //    next_bandits = Math.Clamp(-2 * Math.Log(1 - rng.Randf()), 1, 5); // clamped exponentially distributed random variable
    //    next_storm = Math.Clamp(-1.5 * Math.Log(1 - rng.Randf()), 1, 5);
    //}
//
    //public EncounterRumorContent GetRumor(Town town)
    //{
    //    List<EncounterRumorContent> rumors = [];
    //    foreach (var enc in ActiveEncounters)
    //    {
    //        foreach (var rumor in enc.rumors)
    //        {
    //            if (rumor.Location == town.TownName || rumor.Location == "")
    //            {
    //                enc.rumors.Remove(rumor); // Dont display rumor twice.
    //                return rumor;
    //            }
    //        }
    //    }
    //    return null;
    //}
//
    //public override void _Process(double delta)
    //{
    //    var world_dt = delta * Player.Instance.World.timeScale / (3 * 24) * 10;
    //    next_storm -= world_dt;
    //    next_bandits -= world_dt;
    //    if (ActiveStorm == null && next_storm <= 0) StartStorm();
    //    if (ActiveBandits == null && next_bandits <= 0) StartBandits();
    //    
    //    foreach (var enc in ActiveEncounters)
    //    {
    //        enc.duration -= delta * Player.Instance.World.timeScale / (3 * 24);
    //        if (enc.duration < 0)
    //        {
    //            //GD.Print("Stopped encounter: " + enc.content.Name);
    //            if (enc.handler == ActiveBandits)
    //                EndBandits();
    //            else if (enc.handler == ActiveStorm)
    //                EndStorm();
    //            else
    //                ActiveEncounters.Remove(enc);
    //            break; // quick fix so that removing elements doesnt break the foreach, but skipping a frame is insignificant anyway to these durations.
    //        }
    //    }
    //}
}
*/