using Godot;
using System;

public partial class UIWindow : Control
{
    public virtual string getName() => "Window";
    public virtual void Open() => UIController.Instance.OpenUI(this); // overridden for handling different menu needs
    public virtual void Close() => UIController.Instance.CloseUI(this);

    public WindowHandleUI handleUI;

    public override void _EnterTree()
    {
        base._EnterTree();

        handleUI = GetNode<WindowHandleUI>("handlebar");
    }

    public void windowInput(InputEvent evt)
    {
        if (evt is InputEventMouseButton mb && mb.Pressed) GetParent<Control>().MoveToFront();
        
    }

    public void handleInput(InputEvent evt)
    {
        windowInput(evt);
        // if mouse motion and mouse is held down, move window by motion
        if (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left))
            Position += mm.Relative;
    }

    public void leftSideInput(InputEvent evt)
    {
        // if draggin, add mm.relative.x to left position
        if (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left))
            OffsetLeft += mm.Relative.X;
    }
    public void rightSideInput(InputEvent evt)
    {
        if (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left))
            OffsetRight += mm.Relative.X;
    }
    public void bottomInput(InputEvent evt)
    {
        if (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left))
            OffsetBottom += mm.Relative.Y;
    }
    public void topInput(InputEvent evt)
    {
        if (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left))
            OffsetTop += mm.Relative.Y;
    }
}
