using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using FrooxEngine.UIX;
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

    internal static bool InitializingSelector;
    internal static ComponentSelector? GeneratedSelector;

    public override void OnEngineInit()
    {
        Harmony harmony = new("xyz.jvyden." + nameof(InstantCherryPick));
        Config = GetConfiguration();
        Config?.Save(true);
        harmony.PatchAll();
    }

    [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
    public static void HandleOpenedWindow()
    {
        ComponentSelector? selector = GeneratedSelector;
        if (selector == null)
        {
            Error("Must find selector component");
            return;
        }

        if (Config!.GetValue(FocusUi))
            selector.World.GetScreen().FocusUI(selector.Slot.GetComponent<Canvas>());

        if (Config.GetValue(TextboxFocus))
        {
            // ugly search
            Slot? searchBar = selector.Slot.FindChildInHierarchy("Content").FindChildInHierarchy("Horizontal Layout").Children.FirstOrDefault();
            TextEditor? textEditor = searchBar?.GetComponent<TextEditor>();
            if (searchBar == null || textEditor == null)
            {
                Warn("Could not find CherryPick search bar");
            }
        
            textEditor?.Focus();
        }
    }
}