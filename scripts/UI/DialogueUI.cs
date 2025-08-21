using Godot;

public partial class DialogueUI : Control
{
    Label nameLabel;
    AnimatedSprite2D sprite;

    public override void _EnterTree()
    {
        nameLabel = GetNode<Label>("Panel/NameLabel");
        sprite = GetNode<AnimatedSprite2D>("SpriteControl/AnimatedSprite2D");
    }

    public void openDialogue(Traveller speaking)
    {
        Show();
        nameLabel.Text = speaking.Name;
        sprite.SpriteFrames = speaking.Animation;
        sprite.Play();
    }
    public void revealEncounter(Traveller speaking, EncounterArea encounter)
    {
        openDialogue(speaking);
        
        Player.Instance.moveTo(encounter.Position);
    }
    public void closeDialogue() => Hide();
    
}
