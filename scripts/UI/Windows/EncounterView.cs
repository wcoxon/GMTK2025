using Godot;
using System;

public partial class EncounterView : Control
{

    public UIWindow Window;

    private EncounterArea encounter;

    RichTextLabel titleLabel, descriptionLabel;
    TextureRect imageRect;
    HBoxContainer imageContainer;
    Control buttonContainer;

    public override void _EnterTree()
    {

        Window = GetNode<UIWindow>("Window");
        titleLabel = Window.GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title");
        imageRect = Window.GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect");
        imageContainer = Window.GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer");
        descriptionLabel = Window.GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description");
        buttonContainer = Window.GetNode<Control>("MarginContainer/VSplitContainer/OptionsContainer");
    }

    public void DisplayEncounter(EncounterArea _encounter)
    {
        Window.handleUI.closeButton.Disabled = true;
        Window.handleUI.setTitle("Encounter");
        Window.Open();
        

        encounter = _encounter;

        titleLabel.Text = encounter.data.Title;
        descriptionLabel.Text = encounter.data.Description;
        imageRect.Texture = encounter.data.Image;

        // display option buttons
        for (int i = 0; i < 3; i++)
        {
            var button = buttonContainer.GetChild<Button>(i);
            if (i < encounter.data.Options.Length)
            {
                button.Text = encounter.data.Options[i];
                button.Show();
            }
            else button.Hide();
        }

        //last_state = Player.Instance.State;
        Player.Instance.State = PlayerState.ENCOUNTER;
    }

    public void _on_option_1_pressed() => OnOptionPressed(0);
    public void _on_option_2_pressed() => OnOptionPressed(1);
    public void _on_option_3_pressed() => OnOptionPressed(2);

    private void OnOptionPressed(int index)
    {
        encounter.chooseOption(index);
        Window.Close();
        Player.Instance.State = PlayerState.TRAVEL;
    }
    //public override void Close()
    //{
    //    base.Close();
    //    Player.Instance.State = PlayerState.TRAVEL;
    //}
}
