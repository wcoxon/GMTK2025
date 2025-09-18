using Godot;
using System;

public partial class UIWindow : Control
{


    public WindowHandleUI barUI;
    public Panel bodyUI;

    public virtual void Open()
    {
        PivotOffset = Size / 2;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("popup");

        AudioController.Instance.playAudio("WindowOpen");

        UIController.Instance.OpenUI(this);
    }
    public virtual void Close()
    {
        UIController.Instance.CloseUI(this);
    }

    public virtual void Collapse()
    {
        bodyUI.Visible = !bodyUI.Visible;
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        barUI = GetNode<WindowHandleUI>("handlebar");
        bodyUI = GetNode<Panel>("Body");


        GetNode<Control>("Body/Resizers/leftResizer").GuiInput += leftSideInput;
        GetNode<Control>("Body/Resizers/rightResizer").GuiInput += rightSideInput;
        GetNode<Control>("Body/Resizers/topResizer").GuiInput += topInput;
        GetNode<Control>("Body/Resizers/bottomResizer").GuiInput += bottomInput;

        GetNode<Control>("Body/Resizers/TLResizer").GuiInput += topInput;
        GetNode<Control>("Body/Resizers/TLResizer").GuiInput += leftSideInput;

        GetNode<Control>("Body/Resizers/TRResizer").GuiInput += topInput;
        GetNode<Control>("Body/Resizers/TRResizer").GuiInput += rightSideInput;

        GetNode<Control>("Body/Resizers/BLResizer").GuiInput += bottomInput;
        GetNode<Control>("Body/Resizers/BLResizer").GuiInput += leftSideInput;

        GetNode<Control>("Body/Resizers/BRResizer").GuiInput += bottomInput;
        GetNode<Control>("Body/Resizers/BRResizer").GuiInput += rightSideInput;


        bodyUI.GuiInput += windowInput;
    }
    

    public void windowInput(InputEvent evt)
    {
        if (evt is InputEventMouseButton mb && mb.Pressed) focusWindow();
    }
    public void focusWindow() => MoveToFront();

    // drag bar to move window
    public void barInput(InputEvent evt) => Position += getDrag(evt);

    // drag sides to resize window
    public void leftSideInput(InputEvent evt) => OffsetLeft += getDrag(evt).X;
    public void rightSideInput(InputEvent evt) => OffsetRight += getDrag(evt).X;
    public void bottomInput(InputEvent evt) => OffsetBottom += getDrag(evt).Y;
    public void topInput(InputEvent evt) => OffsetTop += getDrag(evt).Y;

    // if mouse motion with lmb pressed, return motion vector
    Vector2 getDrag(InputEvent evt) => (evt is InputEventMouseMotion mm && Input.IsMouseButtonPressed(MouseButton.Left)) ? mm.Relative : Vector2.Zero;
}
