using Godot;
using Godot.Collections;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


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
    }

    public class EncounterOptionContent
    {
        public string Text { get; set; } = "<option>";
        public string Effect { get; set; } = "";
    }

    public class EncounterRumorContent
    {
        public string Text { get; set; } = "hrm...";
        public string Location { get; set; } = "no-location";
    }

    public class EncounterContent
    {
        public string Name { get; set; } = "no-name";
        public string Class { get; set; } = "no-class";
        public string Location { get; set; } = "no-location";

        public string Title { get; set; } = "no-title";
        public string Image { get; set; } = "";
        public string Description { get; set; } = "";

        public List<EncounterOptionContent> Options { get; set; } = [];
        public List<EncounterRumorContent> Rumors { get; set; } = [];
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
                value -= Execute(target, names, values);
            }
            var (provider, variable_name) = SplitPrefix(target);
            provider.UpdateVariable(variable_name, value);
        }
    }
}
