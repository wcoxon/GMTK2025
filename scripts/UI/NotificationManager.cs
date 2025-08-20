using Godot;
using System;

public partial class NotificationManager : Control
{
    [Export] PackedScene notification_scene;
    [Export] float gap = 10;
    private float offset;
    int active = 0;

    public void AddNotification(string text)
    {
        var notif = notification_scene.Instantiate<Notification>();
        notif.SetText(text);
        notif.Position += Vector2.Down * offset;
        offset += notif.Size.Y + gap;
        AddChild(notif);
        active ++;
    }

    public void OnNotificationRemoved()
    {
        //GD.Print(GetChildCount());
        if (--active == 0)
            offset = 0;
    }
}
