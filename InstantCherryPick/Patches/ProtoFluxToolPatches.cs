using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using Renderite.Shared;

namespace InstantCherryPick.Patches;

[HarmonyPatch(typeof(ProtoFluxTool))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static class ProtoFluxToolPatches
{
    private static readonly MethodInfo OpenNodeBrowser = AccessTools.Method(typeof(ProtoFluxTool), nameof(OpenNodeBrowser)); 
    
    [HarmonyPatch(typeof(ProtoFluxTool), "OnCommonUpdate")]
    [HarmonyPrefix]
    public static void OnCommonUpdatePrefix(ProtoFluxTool __instance)
    {
        if (!InstantCherryPick.Config!.GetValue(InstantCherryPick.Enabled))
            return;
        
        InputInterface input = __instance.Engine.InputInterface;
        if (!input.GetKey(Key.Control) || !input.GetKeyDown(Key.G))
            return;

        InstantCherryPick.InitializingSelector = true;
        OpenNodeBrowser.Invoke(__instance, [null, null]);
        InstantCherryPick.InitializingSelector = false;

        InstantCherryPick.HandleOpenedWindow();
    }

    [HarmonyPatch(typeof(ProtoFluxTool), "OnNodeTypeSelected")]
    [HarmonyPrefix]
    public static void OnNodeTypeSelectedPrefix()
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