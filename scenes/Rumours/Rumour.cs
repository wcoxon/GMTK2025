using System.Collections.Generic;
using System.Linq;
using Godot;

public abstract partial class Rumour
{
    //public string rumourText;

    public List<Traveller> ThoseWhoKnow = new();

    public virtual void focus(){}
    public virtual void reveal(Traveller revealer)
    {
        Player.Instance.traveller.AddRumour(this);
    }

    public void PurgeKnowledge()
    {
        int knowersCount = ThoseWhoKnow.Count;
        
        for (int i = 0; i < knowersCount; i++)
        {
            Traveller knower = ThoseWhoKnow.Last();

            GD.Print($"  purging knowledge from {knower.Name}..");
            knower.RemoveRumour(this);
        }
    }
}
