using Godot;
using System;

public partial class EncounterWindow : UIWindow
{
    private EncounterArea encounter;

    RichTextLabel titleLabel, descriptionLabel;
    TextureRect imageRect;
    HBoxContainer imageContainer;
    Control buttonContainer;

    public override void _EnterTree()
    {
        base._EnterTree();

        barUI.setTitle("Encounter");
        barUI.closeButton.Disabled = true;
        Hide();

        Panel body = GetNode<Panel>("Body");

        titleLabel = body.GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Title");
        imageRect = body.GetNode<TextureRect>("MarginContainer/VSplitContainer/HBoxContainer/TextureRect");
        imageContainer = body.GetNode<HBoxContainer>("MarginContainer/VSplitContainer/HBoxContainer");
        descriptionLabel = body.GetNode<RichTextLabel>("MarginContainer/VSplitContainer/Description");
        buttonContainer = body.GetNode<Control>("MarginContainer/VSplitContainer/OptionsContainer");

        buttonContainer.GetChild<Button>(0).Pressed += () => OnOptionPressed(0);
        buttonContainer.GetChild<Button>(1).Pressed += () => OnOptionPressed(1);
        buttonContainer.GetChild<Button>(2).Pressed += () => OnOptionPressed(2);
    }

    public void DisplayEncounter(EncounterArea _encounter)
    {
        Open();

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
    }

    private void OnOptionPressed(int index)
    {
        encounter.chooseOption(index);

        base.Close();
        UIController.Instance.timeControlPanel.enable();
        UIController.Instance.timeControlPanel.Speed1();
        Player.Instance.State = PlayerState.TRAVEL;
    }

    public override void Open()
    {
        base.Open();
        UIController.Instance.timeControlPanel.Pause();
        UIController.Instance.timeControlPanel.disable();
        Player.Instance.State = PlayerState.ENCOUNTER;
    }

    public override void Close(){}
}
