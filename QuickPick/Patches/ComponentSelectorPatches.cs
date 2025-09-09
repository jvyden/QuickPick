using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using HarmonyLib;
using Renderite.Shared;

namespace QuickPick.Patches;

[HarmonyPatch(typeof(ComponentSelector))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class ComponentSelectorPatches
{
    [HarmonyPatch(typeof(ComponentSelector), "OnAttach")]
    [HarmonyPrefix]
    public static void ComponentSelectorOnAttachPrefix(ComponentSelector __instance)
    {
        bool shift = __instance.Engine.InputInterface.GetKey(Key.Shift);
        if (!QuickPick.InitializingSelector && !shift)
            return;

        QuickPick.GeneratedSelector = __instance;
        __instance.StartTask(async () =>
        {
            await default(NextUpdate);
            QuickPick.HandleOpenedWindow();
        });
    }

    [HarmonyPatch(typeof(ComponentSelector), "OnAddComponentPressed")]
    [HarmonyPostfix]
    public static void OnAddComponentPressedPostfix()
    {
        if (!QuickPick.Config!.GetValue(QuickPick.DestroySearch)) return;

        QuickPick.GeneratedSelector?.Slot.Destroy();

        if (QuickPick.Config.GetValue(QuickPick.FocusUi))
        {
            ScreenController? screen = QuickPick.GeneratedSelector?.World.GetScreen();
            screen?.UnfocusUI();

            // HACK: since view targeting isn't a stack, we need to preserve the last UI on our own
            // if we force UI to be the previous target, we can get the user stuck in UI targeting mode 
            if (QuickPick.PreviousUI != null)
            {
                screen?.FocusUI(QuickPick.PreviousUI);
                QuickPick.PreviousUI = null;
            }
        }
        
        QuickPick.GeneratedSelector = null;
    }
    
}