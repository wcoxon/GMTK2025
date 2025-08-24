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

    public void revealEncounter(Traveller speaking, EncounterArea encounter)
    {
        openDialogue(speaking);
        Player.Instance.moveTo(encounter.Position);
    }

    public void openDialogue(Traveller speaking)
    {
        Player.Instance.UI.townPanel.Hide();
        Player.Instance.UI.OpenUI(this);

        nameLabel.Text = speaking.Name;
        sprite.SpriteFrames = speaking.Animation;
        sprite.Play();
    }
    public void closeDialogue() {
        Player.Instance.UI.townPanel.Show();
        Player.Instance.UI.CloseUI(this);
    }
    
}
