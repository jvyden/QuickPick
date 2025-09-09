using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using Renderite.Shared;

namespace InstantCherryPick.Patches;

[HarmonyPatch(typeof(ProtoFluxTool))]
[SuppressMessage("ReSharper", "InconsistentNaming")]
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

        OpenNodeBrowser.Invoke(__instance, [null, null]);

        InstantCherryPick.HandleOpenedWindow(__instance);
    }
}