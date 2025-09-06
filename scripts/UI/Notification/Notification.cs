using Godot;
using System;

public partial class Notification : Panel
{
    float Alive = 3;
    public void SetText(string text, float alive = 3)
    {
        GetNode<RichTextLabel>("MarginContainer/TextEdit").Text = text;
        Alive = alive;
    }

    public override void _Process(double delta)
    {
        Alive -= (float)delta;
        Modulate = new Color(1f, 1f, 1f, Mathf.Clamp(Alive, 0f, 1f));
        if (Alive < 1)
            Position += Vector2.Right * (float) delta * 50;
        if (Alive < 0)
        {
            QueueFree();
            GetParent<NotificationManager>().OnNotificationRemoved();
        }
    }
}
