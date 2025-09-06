using Godot;
using System;

public enum PlayerState { TOWN, PLAN, TRAVEL, ENCOUNTER }
public enum Item { BROTH, PLASTICS, EVIL_WATER }
public partial class Game : GodotObject
{
    public static string[] itemNames = ["Broth", "Plastics", "Evil Water"];
    public static int[] itemBaseValues = [10, 10, 10];//[5, 12, 10];
}
