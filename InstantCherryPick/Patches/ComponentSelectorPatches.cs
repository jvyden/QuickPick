using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using HarmonyLib;
using Renderite.Shared;

namespace InstantCherryPick.Patches;

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
        if (!InstantCherryPick.InitializingSelector && !shift)
            return;

        InstantCherryPick.GeneratedSelector = __instance;
        __instance.StartTask(async () =>
        {
            await default(NextUpdate);
            InstantCherryPick.HandleOpenedWindow();
        });
    }

    [HarmonyPatch(typeof(ComponentSelector), "OnAddComponentPressed")]
    [HarmonyPostfix]
    public static void OnAddComponentPressedPostfix()
    {
        if (!InstantCherryPick.Config!.GetValue(InstantCherryPick.DestroySearch)) return;

        InstantCherryPick.GeneratedSelector?.Slot.Destroy();

        if (InstantCherryPick.Config.GetValue(InstantCherryPick.FocusUi))
        {
            ScreenController? screen = InstantCherryPick.GeneratedSelector?.World.GetScreen();
            screen?.UnfocusUI();

            // HACK: since view targeting isn't a stack, we need to preserve the last UI on our own
            // if we force UI to be the previous target, we can get the user stuck in UI targeting mode 
            if (InstantCherryPick.PreviousUI != null)
            {
                screen?.FocusUI(InstantCherryPick.PreviousUI);
                InstantCherryPick.PreviousUI = null;
            }
        }
        
        InstantCherryPick.GeneratedSelector = null;
    }
    
}