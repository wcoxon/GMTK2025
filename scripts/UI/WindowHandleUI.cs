using Godot;
using System;

public partial class WindowHandleUI : Panel
{
    UIWindow window;

    Panel bodyPanel;

    [Export] Label titleLabel;
    [Export] public Button closeButton;
    [Export] public Button collapseButton;

    public override void _EnterTree()
    {
        base._EnterTree();

        window = GetParent<UIWindow>();
        bodyPanel = window.GetNode<Panel>("Body");

        // set close button to close window
        closeButton.Pressed += window.Close;
        collapseButton.Pressed += window.Collapse;//toggleBodyVisible;

        GuiInput += window.barInput;
        GuiInput += window.windowInput;
    }

    public void setTitle(string text) => titleLabel.Text = text;

    //public void toggleBodyVisible()
    //{
    //    bodyPanel.Visible = !bodyPanel.Visible;
    //}
    
}
