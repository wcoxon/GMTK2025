using Godot;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

//encounter UI controller?
public partial class EncounterView : Panel
{
    private EncounterArea encounter;
    private GameState last_state;


    RichTextLabel titleLabel, descriptionLabel;
    TextureRect imageRect;
    HBoxContainer imageContainer;

    public override void _EnterTree()
    {
        titleLabel = GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title");
        imageRect = GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect");
        imageContainer = GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer");
        descriptionLabel = GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description");
    }

    public void DisplayEncounter(EncounterArea _encounter, Action post = null)
    {
        encounter = _encounter;

        Show();

        titleLabel.Text = encounter.data.Title; // display encounter title
        descriptionLabel.Text = encounter.data.Description;

        // load image if any
        
        imageRect.Texture = encounter.data.Image;
        
        // display option buttons
        for (int i = 0; i < encounter.data.Options.Length; i++)
        {
            var button = GetNode<Button>($"MarginContainer/VSplitContainer/Option{i + 1}");

            button.Text = encounter.data.Options[i];
            button.Show();
        }

        last_state = Player.Instance.State;
        Player.Instance.State = GameState.ENCOUNTER;
    }
    public void closeEncounter()
    {
        Hide();
        for (int i = 0; i < encounter.data.Options.Length; i++)
        {
            GetNode<Button>($"MarginContainer/VSplitContainer/Option{i+1}").Hide();
        }

        // return player to their previous state
        Player.Instance.State = last_state; // couldn't this always be assumed to be travelling?
    }
    private void OnOptionPressed(int index)
    {
        encounter.chooseOption(index);
        closeEncounter();
        //EncounterManager.Instance.ApplyEffects(content.Options[id].Effect);
    }

    public void _on_option_1_pressed() => OnOptionPressed(0);
    public void _on_option_2_pressed() => OnOptionPressed(1);
    public void _on_option_3_pressed() => OnOptionPressed(2); 
}
