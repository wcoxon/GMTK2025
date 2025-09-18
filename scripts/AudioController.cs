using Godot;
using System;

public partial class AudioController : Node
{
	public static AudioController Instance;
	public AudioStreamPlayer clickDown, clickUp;

	public override void _EnterTree()
	{
		base._EnterTree();
		Instance = this;

		clickDown = GetNode<AudioStreamPlayer>("ClickDown");
		clickUp = GetNode<AudioStreamPlayer>("ClickUp");

	}

	public void playAudio(string name)
	{
		GetNode<AudioStreamPlayer>(name).Play();
	}

}
