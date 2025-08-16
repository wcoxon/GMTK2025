using Godot;
using Godot.Collections;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class EncounterOptionContent
{
    public string Text { get; set; } = "<option>";
    public string Effect { get; set; } = "";
}

public class EncounterRumorContent
{
    public string Text { get; set; } = "hrm...";
    public string Location { get; set; } = "";
}

public class EncounterContent
{
    // Internal name of the encounter.
    public string Name { get; set; } = "no-name";
    // What class should handle these encounters.
    public string Class { get; set; } = "no-class";
    // Where encounter will take place (implementation left for class)
    public string Location { get; set; } = "no-location";
    // Title of encounter popup.
    public string Title { get; set; } = "no-title";
    // Image in encounter popup.
    public string Image { get; set; } = "";
    // Description in encounter popup.
    public string Description { get; set; } = "";
    // Options in encounter popup.
    public List<EncounterOptionContent> Options { get; set; } = [];
    // Rumors to spread while encounter is active.
    public List<EncounterRumorContent> Rumors { get; set; } = [];
    // Chance for encounter to happen.
    public double Chance { get; set; } = 1.0;
    // Duration of encounter in days.
    public double Duration { get; set; } = 1.0;

}

public partial class EncounterManager : Node
{
    public static EncounterManager Instance { get; private set; }

    public List<EncounterContent> encounters = [];
    public override void _Ready()
    {
        Instance = this;

        var dir = DirAccess.Open(EVENT_DIRECTORY);
        dir.ListDirBegin();

        foreach (var filename in dir.GetFiles())
        {
            var file = FileAccess.Open(EVENT_DIRECTORY + filename, FileAccess.ModeFlags.Read);
            encounters.AddRange(deserializer.Deserialize<List<EncounterContent>>(file.GetAsText()));
        }
        foreach (var enc in encounters)
        {
            //GD.Print(enc.Location);
        }
    }

    public List<EncounterContent> GetFromClass(string cls)
    {
        List<EncounterContent> ret = [];
        foreach (var content in encounters)
        {
            if (content.Class == cls)
                ret.Add(content);
        }
        return ret;
    }

    static IDeserializer deserializer = new DeserializerBuilder()
        .IgnoreUnmatchedProperties()
        .WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

    const string EVENT_DIRECTORY = "res://encounters/";

    static RegEx effect_syntax = RegEx.CreateFromString(
        "\\s*([A-Za-z_][A-Za-z0-9_]*)\\s*(=|\\+=|-=)(.*)");

    public interface IVariableProvider
    {
        string VariablesPrefix();
        List<(string, double)> GetVariables();
        void UpdateVariable(string name, double value);
    }

    List<IVariableProvider> providers = [];
    public void AddProvider(IVariableProvider provider)
    {
        providers.Add(provider);
    }

    public (string[], Array) GetVariables()
    {
        List<string> names = [];
        var values = new Array();
        foreach (var provider in providers)
        {
            var prefix = provider.VariablesPrefix() + "_";
            foreach (var (name, value) in provider.GetVariables())
            {
                names.Add(prefix + name);
                values.Add(value);
            }
        }
        return (names.ToArray(), values);
    }

    public double Execute(string expr_text, string[] names, Array values)
    {
        var expr = new Expression();
        expr.Parse(expr_text, names);
        return expr.Execute(values).As<double>();
    }

    private (IVariableProvider, string) SplitPrefix(string name)
    {
        foreach (var provider in providers)
        {
            var prefix = provider.VariablesPrefix() + "_";
            if (name.StartsWith(prefix))
            {
                var variable_name = name.Substring(prefix.Length);
                return (provider, variable_name);
            }
        }
        throw new KeyNotFoundException("Not found prefix in providers for: " + name);
    }

    public void ApplyEffects(string effects)
    {
        var (names, values) = GetVariables();
        foreach (var effect in effects.Split([';', '\n']))
        {
            if (effect.Trim().Length == 0) continue;
            var match = effect_syntax.Search(effect);
            var target = match.Strings[1];
            var opp = match.Strings[2];
            var expr = match.Strings[3];
            var value = Execute(expr, names, values);
            if (opp == "+=")
            {
                value += Execute(target, names, values);
            }
            else if (opp == "-=")
            {
                value = Execute(target, names, values) - value;
            }
            var (provider, variable_name) = SplitPrefix(target);
            provider.UpdateVariable(variable_name, value);
        }
    }
}
