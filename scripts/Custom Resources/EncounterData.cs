using Godot;

// stores info about encounter ig
// title, description, image, options
[GlobalClass]
public partial class EncounterData : Resource
{
    [Export] public string Title;
    [Export(PropertyHint.MultilineText)] public string Description;
    [Export] public Texture2D Image;
    [Export] public string[] Options;
}