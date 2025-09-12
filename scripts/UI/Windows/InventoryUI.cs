using Godot;
using System;

// a virtual program ?
// toolbar is to run programs, the program is more top level, contains a generic window 
// thus prefab doesn't have to be scene root
// and uh yeah right this isn't a stupid idea right

// what does this solve regarding the player inventory vs others inventory
// and toolbar open thing

// this stores subject,
// so we have set the subject of the program and the window stuff will update
// but the toolbar only sees this as another program, doesn't set subject
// i don't even liike the toolbars,
// like think of it like how taskbars work irl
// when you open a thing its either empty or last state

//hmm i mean yeah maybe to just have a button for open player stuff
// rather than a button for open stuff 
// for now at the very least

public partial class InventoryUI : Control
{
	//private Traveller subject;
	//public Traveller Subject
	//{
	//	get => subject;
	//	set
	//	{
	//		subject = value;
	//		updateUI();
	//	}
	//}

	private Traveller subject;

	public UIWindow Window;

	[Export] Label moneyLabel;
	[Export] VBoxContainer rowsContainer;

	public override void _EnterTree()
	{
		base._EnterTree();

		Window = GetNode<UIWindow>("Window");
		Window.Hide(); // so we can have it visible in editor but not at game start
	}

	
	public void handleInput(InputEvent evt)
	{
		//if (evt is InputEventMouseButton mb && mb.Pressed) MoveToFront();
	}


	public void updateUI()
	{
		moneyLabel.Text = $"Crumbs: {subject.Money}";
		foreach (StockUI row in rowsContainer.GetChildren()) row.updateRow(subject);
	}
	public void OpenPlayerInventory()
	{
		OpenInventory(Player.Instance.traveller);
	}
	public void OpenInventory(Traveller character)
	{
		subject = character;
		

		Window.handleUI.setTitle($"INVENTORY - {character.CharacterName}");
		Window.Open();
		
		updateUI();
	}

	public void Close()
	{
		Window.Close();
	}
}
