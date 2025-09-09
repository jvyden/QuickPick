using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;

namespace QuickPick;

public partial class QuickPick : ResoniteMod
{
    public override string Name => nameof(QuickPick);
    public override string Author => "jvyden";
    public override string Version => typeof(QuickPick).Assembly.GetName().Version?.ToString() ?? "0.0.0";
    public override string Link => "https://github.com/jvyden/" + nameof(QuickPick);
    
    public static ModConfiguration? Config { get; private set; }

    internal static bool InitializingSelector;
    internal static ComponentSelector? GeneratedSelector;
    internal static IUIInterface? PreviousUI;

    public override void OnEngineInit()
    {
        Harmony harmony = new("xyz.jvyden." + nameof(QuickPick));
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
        {
            ScreenController screen = selector.World.GetScreen();

            if (screen.ActiveTargetting.Target is UI_TargettingController ui)
                PreviousUI = ui.Target.Target;

            screen.FocusUI(selector.Slot.GetComponent<Canvas>());
        }

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