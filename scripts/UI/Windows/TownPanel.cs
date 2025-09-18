using Godot;

// town menu
public partial class TownPanel : UIWindow
{
    // populates UI contents with information about given town

    [Export] Label nameLabel, populationLabel, wealthLabel;

    [Export] AnimatedSprite2D[] visitorSprites;
    [Export] MarketTable marketTable;

    [Export] Button tradeButton, rumourButton;

    private Town town;
    public Town Town
    {
        get => town;
        set
        {
            town = value;
            Player.Instance.UI.tradeUI.Town = Town;
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        Hide();

        tradeButton.Pressed += Trade;
        rumourButton.Pressed += getRumour;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Visible && Town is not null) updateUI(); // keep UI up to date with changing town stats // can't we only update when a stat updates or smth idk like on tick
    }

    public void updateUI()
    {
        //just need to update the dynamic ui to the latest values, town name won't need updating here it's set only when town is selected
        populationLabel.Text = $"Population: {town.Population}";
        wealthLabel.Text = $"Wealth: {town.Wealth}";

        updateVisitors();
        marketTable.updateTable(Town);
    }

    public void updateVisitors()
    {
        for (int i = 0; i < visitorSprites.Length; i++)
        {
            if (i >= Town.Visitors.Count)
            {
                visitorSprites[i].SpriteFrames = null;
                continue;
            }

            visitorSprites[i].SpriteFrames = Town.Visitors[i].Animation;
            visitorSprites[i].Play();
        }
    }

    public void updateActions()
    {
        bool onTown = Player.Instance.State == PlayerState.TOWN;
        bool onSelected = Player.Instance.traveller.Town == Town;

        tradeButton.Disabled = rumourButton.Disabled = !(onTown && onSelected);
    }

    public void Trade() => UIController.Instance.tradeUI.Open();
    public void getRumour()
    {
        var shuffledVisitors = Town.Visitors.Duplicate();
        shuffledVisitors.Shuffle();

        foreach (Traveller visitor in shuffledVisitors)
        {
            foreach (Rumour rumour in visitor.knownRumours)
            {
                if (Player.Instance.traveller.knownRumours.Contains(rumour)) continue;
                
                Close();
                rumour.reveal(visitor);
                return;
            }
        }
    }

    public void OpenPlayerTown() => OpenTown(Player.Instance.traveller.Town);
    
    public void OpenTown(Town town)
    {
        Town = town;
        barUI.setTitle($"TOWN - {Town.TownName}");

        Open();


        nameLabel.Text = Town.TownName; // only update name when town changed
        updateUI(); // update stats UI
        updateActions(); // update buttons
    }
}
