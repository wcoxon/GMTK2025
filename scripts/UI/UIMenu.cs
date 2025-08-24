using Godot;
using System;

public partial class UIMenu : Panel
{
    public virtual string getName() => "[UI menu]";

    public virtual void Open() => Player.Instance.UI.OpenUI(this); // overridden for handling different menu needs
    public virtual void Close() => Player.Instance.UI.CloseUI(this);

}
