using Godot;

public partial class DialogueWindow : UIWindow
{
    AnimatedSprite2D sprite;

    public override void _EnterTree()
    {
        base._EnterTree();

        Hide();

        sprite = GetNode<AnimatedSprite2D>("SpriteContainer/AnimatedSprite2D");
    }

    public void revealEncounter(Traveller speaking, EncounterArea encounter)
    {
        openDialogue(speaking);
        Player.Instance.moveTo(encounter.Position);
    }

    public void openDialogue(Traveller speaking)
    {
        barUI.setTitle(speaking.CharacterName);
        sprite.SpriteFrames = speaking.Animation;
        sprite.Play();
        Open();
    }
}
