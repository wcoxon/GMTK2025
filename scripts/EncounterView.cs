using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class EncounterView : Panel
{
    private EncounterContent content;
    private Action post_action;

    private GameState last_state;


    RichTextLabel titleLabel, descriptionLabel;
    TextureRect imageRect;
    HBoxContainer imageContainer;

    public override void _EnterTree()
    {
        base._EnterTree();

        titleLabel = GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title");
        imageRect = GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect");
        imageContainer = GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer");
        descriptionLabel = GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description");
    }


    public override void _Ready()
    {
        // Display any encounter with class testnow immediately.
        var contents = EncounterManager.Instance.GetFromClass("testnow");
        if (contents.Any()) DisplayEncounter(contents[0]);
    }

    public void DisplayEncounter(EncounterContent _content, Action post = null)
    {
        content = _content;
        Visible = true;

        titleLabel.Text = content.Title; // display encounter title

        // load image if any
        if (content.Image != "")
        {
            var texture = ImageTexture.CreateFromImage(Image.LoadFromFile(content.Image));
            imageRect.Texture = texture;
            imageContainer.Visible = true;
        }
        else imageContainer.Visible = false;
        
        // display encounter description
        descriptionLabel.Text = content.Description;

        // display option buttons
        int i = 1;
        foreach (var option in content.Options)
        {
            var button = GetNode<Button>("MarginContainer/VSplitContainer/Option" + i++);
            button.Visible = true;
            button.Text = option.Text;
        }

        // store action
        post_action = post;

        last_state = Player.Instance.State;
        Player.Instance.State = GameState.ENCOUNTER;
    }

    private void OnOptionPressed(int id)
    {
        EncounterManager.Instance.ApplyEffects(content.Options[id].Effect);
        Visible = false;
        for (int i = 1; i <= 3; i++)
        {
            GetNode<Button>("MarginContainer/VSplitContainer/Option" + i).Visible = false;
        }

        Player.Instance.State = last_state;
        //PlayerView.Instance.PlayWorldSpeed(); // should play anyway.. you know when you transition state
        if (post_action != null) post_action();
    }

    public void _on_option_1_pressed()
    {
        OnOptionPressed(0);
    }

    public void _on_option_2_pressed()
    {
        OnOptionPressed(1);
    }
    
    public void _on_option_3_pressed()
    {
        OnOptionPressed(2); // u can send arguments within signals u know
    }
}
