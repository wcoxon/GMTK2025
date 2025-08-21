using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract partial class Rumour
{
    //public string rumourText;

    //public double dayRecieved;
    //public double duration; //in whole days.
    //public double expirationDay => dayRecieved + duration;

    public List<Traveller> ThoseWhoKnow = new();

    public virtual void reveal(Traveller revealer){}

    public void PurgeKnowledge()
    {
        int knowersCount = ThoseWhoKnow.Count;
        
        for (int i = 0; i < knowersCount; i++)
        {
            Traveller knower = ThoseWhoKnow.Last();

            GD.Print($"  purging knowledge from {knower.Name}..");
            knower.knownRumours.Remove(this);
            ThoseWhoKnow.Remove(knower);
        }
    }
}
