using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class EncounterView : Panel
{
    private EncounterManager.EncounterContent content;
    private Action post_action;

    private GameState last_state;

    public override void _Ready()
    {
        // Display any encounter with class testnow immediately.
        var contents = EncounterManager.Instance.GetFromClass("testnow");
        if (contents.Any())
            DisplayEncounter(contents[0]);
    }

    public void DisplayEncounter(EncounterManager.EncounterContent content, Action post = null)
    {
        this.content = content;
        Visible = true;

        GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title").Text = content.Title;
        if (content.Image != "")
        {
            var texture = ImageTexture.CreateFromImage(Image.LoadFromFile(content.Image));
            GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect").Texture = texture;
            GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer").Visible = true;
        }
        else
        {
            GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer").Visible = false;
        }
        GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description").Text = content.Description;
        int i = 1;
        foreach (var option in content.Options)
        {
            var button = GetNode<Button>("MarginContainer/VSplitContainer/Option" + i++);
            button.Visible = true;
            button.Text = option.Text;
        }

        post_action = post;
        PlayerView.Instance.PauseWorldSpeed();
        last_state = PlayerView.Instance.State;
        PlayerView.Instance.State = GameState.ENCOUNTERING;
    }

    private void OnOptionPressed(int id)
    {
        EncounterManager.Instance.ApplyEffects(content.Options[id].Effect);
        Visible = false;
        for (int i = 1; i <= 3; i++)
        {
            GetNode<Button>("MarginContainer/VSplitContainer/Option" + i).Visible = false;
        }

        PlayerView.Instance.State = last_state;
        PlayerView.Instance.PlayWorldSpeed();
        if (post_action != null)
            post_action();
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
        OnOptionPressed(2);
    }
}
