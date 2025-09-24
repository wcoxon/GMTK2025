using Godot;
using System;

public partial class UIWindow : Control
{
    public UIWindowTitleBar barUI;
    public Panel bodyUI, borderUI;

    public virtual void Open()
    {
        PivotOffset = Size / 2;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("popup");

        AudioController.Instance.playAudio("WindowOpen");

        UIController.Instance.OpenUI(this);
    }
    public virtual void Close()
    {
        
        PivotOffset = Size / 2;
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Close");

        AudioController.Instance.playAudio("WindowClose");

        //UIController.Instance.CloseUI(this);
    }

    public virtual void OnFinishClose()
    {
        UIController.Instance.CloseUI(this);
    }

    public virtual void Collapse()
    {
        bodyUI.Visible = !bodyUI.Visible;
        borderUI.Visible = bodyUI.Visible;
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        barUI = GetNode<UIWindowTitleBar>("TitleBar");
        bodyUI = GetNode<Panel>("Body");
        borderUI = GetNode<Panel>("Resizers");


        GetNode<Control>("Resizers/leftResizer").GuiInput += leftSideInput;
        GetNode<Control>("Resizers/rightResizer").GuiInput += rightSideInput;
        GetNode<Control>("Resizers/topResizer").GuiInput += topInput;
        GetNode<Control>("Resizers/bottomResizer").GuiInput += bottomInput;

        GetNode<Control>("Resizers/TLResizer").GuiInput += topInput;
        GetNode<Control>("Resizers/TLResizer").GuiInput += leftSideInput;

        GetNode<Control>("Resizers/TRResizer").GuiInput += topInput;
        GetNode<Control>("Resizers/TRResizer").GuiInput += rightSideInput;

        GetNode<Control>("Resizers/BLResizer").GuiInput += bottomInput;
        GetNode<Control>("Resizers/BLResizer").GuiInput += leftSideInput;

        GetNode<Control>("Resizers/BRResizer").GuiInput += bottomInput;
        GetNode<Control>("Resizers/BRResizer").GuiInput += rightSideInput;


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
