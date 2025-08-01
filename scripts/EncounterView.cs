using Godot;
using System;

public partial class EncounterView : Panel
{
    private EncounterManager.EncounterContent content;

    public override void _Ready()
    {
        // Display any encounter with class testnow immediately.
        foreach (var encounter in EncounterManager.Instance.encounters)
        {
            if (encounter.Class == "testnow")
            {
                DisplayEncounter(encounter);
                break;
            }
        }
    }

    public void DisplayEncounter(EncounterManager.EncounterContent content)
    {
        this.content = content;
        Visible = true;

        GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title").Text = content.Title;
        var texture = ImageTexture.CreateFromImage(Image.LoadFromFile(content.Image));
        GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect").Texture = texture;
        GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description").Text = content.Description;
        int i = 1;
        foreach (var option in content.Options)
        {
            var button = GetNode<Button>("MarginContainer/VSplitContainer/Option" + i++);
            button.Visible = true;
            button.Text = option.Text;
        }
    }

    private void OnOptionPressed(int id)
    {
        EncounterManager.Instance.ApplyEffects(content.Options[id].Effect);
        Visible = false;
        for (int i = 1; i <= 3; i++)
        {
            GetNode<Button>("MarginContainer/VSplitContainer/Option"+i).Visible = false;
        }
        // TODO: Resume game.
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
