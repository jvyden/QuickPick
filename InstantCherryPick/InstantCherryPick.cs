using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;

namespace InstantCherryPick;

public partial class InstantCherryPick : ResoniteMod
{
    public override string Name => nameof(InstantCherryPick);
    public override string Author => "jvyden";
    public override string Version => typeof(InstantCherryPick).Assembly.GetName().Version?.ToString() ?? "0.0.0";
    public override string Link => "https://github.com/jvyden/" + nameof(InstantCherryPick);
    
    public static ModConfiguration? Config { get; private set; }

    public override void OnEngineInit()
    {
        Harmony harmony = new("xyz.jvyden." + nameof(InstantCherryPick));
        Config = GetConfiguration();
        Config?.Save(true);
        harmony.PatchAll();
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public static void HandleOpenedWindow(Worker worker)
    {
        // Match both vanilla name and CherryPick flair
        // https://github.com/BlueCyro/CherryPick/blob/master/ComponentSelector_Patcher.cs#L38
        Slot? selector = worker.LocalUserSpace.FindChild(s => s.Name == "Node Browser" || s.Name == "Component Selector" || s.Name.Contains("CherryPick"), 0);
        if (selector == null)
        {
            Error("Must find selector slot");
            return;
        }

        if (selector.GetComponent(typeof(ComponentSelector), true) == null)
        {
            Error($"Must find selector slot with {nameof(ComponentSelector)}");
            return;
        }
        
        // Debug($"Found selector: {selector}");

        if (Config!.GetValue(FreecamFocus))
            selector.FocusFreecam();
        
        Slot? searchBar = selector.FindChildInHierarchy("Content").FindChildInHierarchy("Horizontal Layout").Children.FirstOrDefault();
        TextEditor? textEditor = searchBar?.GetComponent<TextEditor>();
        if (searchBar == null || textEditor == null)
        {
            Warn("Could not find CherryPick search bar");
            return;
        }
        
        if(Config.GetValue(TextboxFocus))
            textEditor.Focus();
    }
}