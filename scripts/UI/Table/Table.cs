using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

public partial class Table : Control
{

    int sortedColumn = 0;
    bool sortAscending = true;

    [Export] HBoxContainer headersContainer;
    [Export] VBoxContainer rowsContainer;
    [Export] TextureRect sortIcon;

    public override void _EnterTree()
    {
        base._EnterTree();

        foreach (Label headerLabel in headersContainer.GetChildren())
        {
            headerLabel.GuiInput += (evt) => headerInput(evt, headerLabel.GetIndex());
        }
    }

    void headerInput(InputEvent evt, int columnIndex)
    {
        if(evt is InputEventMouseButton mb && Input.IsActionJustPressed("select")) SortColumn(columnIndex);
    }

    public void SortColumn(int columnIndex)
    {
        if (columnIndex == sortedColumn)
        {
            // this column is currently sorted
            // flip order
            sortAscending = !sortAscending;
        }
        else
        {
            sortAscending = true;
        }

        sortIcon.FlipV = !sortAscending;

        sortIcon.GetParent().RemoveChild(sortIcon);
        headersContainer.GetChild(columnIndex).AddChild(sortIcon); // move icon to this column


        for (int passIndex = 1; passIndex < rowsContainer.GetChildCount(); passIndex++)
        {
            for (int rowIndex = 0; rowIndex < rowsContainer.GetChildCount() - passIndex; rowIndex++)
            {
                // compare row to next
                HBoxContainer row = rowsContainer.GetChild<HBoxContainer>(rowIndex);
                HBoxContainer nextRow = rowsContainer.GetChild<HBoxContainer>(rowIndex + 1);

                Node field = row.GetChild(columnIndex);
                Node nextField = nextRow.GetChild(columnIndex);

                if (CompareFields(field, nextField) == (sortAscending ? 1 : -1))
                {
                    rowsContainer.MoveChild(row, rowIndex + 1);
                }
            }
        }
        sortedColumn = columnIndex;
    }

    public int CompareFields(Node field1, Node field2)
    {
        if (field1 is Label label1 && field2 is Label label2)
        {
            if (label1.Text.IsValidInt()) return label1.Text.ToInt().CompareTo(label2.Text.ToInt());

            return label1.Text.CompareTo(label2.Text);
        }

        return 0;
    }
}
