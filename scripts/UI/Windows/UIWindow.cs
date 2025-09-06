using Godot;
using System;

public partial class UIWindow : Control
{
    public virtual string getName() => "";
    public virtual void Open() => Player.Instance.UI.OpenUI(this); // overridden for handling different menu needs
    public virtual void Close() => Player.Instance.UI.CloseUI(this);
}
