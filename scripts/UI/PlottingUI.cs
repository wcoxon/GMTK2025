using Godot;
using System;

public partial class PlottingUI : Panel
{
    // could show stuff like distance in this ui too idk

    public void Open() => Show();

    public void Embark()
    {
        Player.Instance.embark(); // i hope we can move that logic off the player anyway
        Hide();
    }
    public void Cancel()
    {
        Player.Instance.cancelPlot();
        Hide();
    }
}
