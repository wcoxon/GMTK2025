using Godot;
using System;

public partial class WindowHandleUI : Panel
{
    UIWindow window;

    [Export] Label titleLabel;
    [Export] public Button closeButton;

    public override void _EnterTree()
    {
        base._EnterTree();

        window = GetParent<UIWindow>();

        // update label
        titleLabel.Text = window.getName();
        // set close button to close window
        closeButton.Pressed += window.Close;
        GuiInput += window.handleInput;
    }

    public void setTitle(string text)
    {
        titleLabel.Text = text;
    }
}
