using Godot;
using System;

public partial class Table : Control
{
    // this will be THE modular table system thing we need to display and read data in table form



    // give row to table, row data


    // inventory
    //  item, quantity

    // market display
    //  item, quantity, ...,  price

    // trading
    //  item, quantity, player quantity, price, offer


    // i can make prefabs for types of row though right
    // yeah but then if you make a script for interfacing with each type of row you have to use different logic to read from them too


    // the idea is to split the market info into its own scene and have a script that manages that table,
    // then it's like, why not use the same script for all the tables like that
    // and then the head of the scene exposes all the stuff in the form of easy to read data
    // same scene even. and yeah this would make it easier to add new columns and stuff too if we want


    // so what form does this data take first off
    //  well a 'table' can be stored as a list of objects, that's a way i like. structurally i call that a table anyway, a collection of records with set fields

    // but the table's fields can vary in our case, depending on what we're representing. so the data type we collect would vary and how they are read


    //  how about this, how are we ideally going to be able to send data into it


    //  well the inventory, you provide a traveller
    //  the market, you provide a town
    //  trading, you provide a traveller AND a town


    // so naturally these are going to need some preparation logic somewhere to bring the data forwards


    // look, right now the town panel goes through each row setting the field values
    
    
    




}
