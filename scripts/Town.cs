using Godot;
using System;
using System.Collections.Generic;

public partial class Town : Node3D
{
    // towns have production (item, rate)
    // consumption (item, rate) (could just use negative production)
    // population (scales production and consumption)
    // stock (list of (item, quantity)), price is calculated in getPrice(item) using quantity and consumption of the item


    //refs, replace with selection in _Ready later probab
    [Export] MeshInstance3D mesh;


    public bool Selected
    {
        set
        {
            mesh.SetInstanceShaderParameter("width", value ? 0.2 : 0);
        }
    }

    // scooping town data out of custom resource because then we can save town data onto resource files
    [Export] TownData data;
    public string TownName { get => data.townName; }
    public int Population { get => data.population; }
    public Godot.Collections.Dictionary<string, int> Stock { get => data.stock; } // we prob don't wanna use a string to denote items, this is just a stub, same for int quantity

    Dictionary<Item, float> deltaStock = new(); // store production and consumption here as change in items over time?

    public void _on_area_3d_input_event(Node cam, InputEvent evt, Vector3 evtPos, Vector3 normal, int shapeIndex)
    {
        if (evt is InputEventMouseButton mbEvent)
        {
            // if the area was clicked on, this town is selected (should use actions instead so u can look for isActionPressed)
            select();
        }
    }

    public void select()
    {
        PlayerView.instance.SelectedTown = this;
    }

    void updateStock(float deltaTime)
    {
        // produce or consume items over time based on deltaStock
    }
}
