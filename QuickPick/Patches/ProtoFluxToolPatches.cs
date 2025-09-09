using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using HarmonyLib;
using Renderite.Shared;

namespace QuickPick.Patches;

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
        if (!QuickPick.Config!.GetValue(QuickPick.Enabled))
            return;
        
        InputInterface input = __instance.Engine.InputInterface;
        if (!input.GetKey(Key.Control) || !input.GetKeyDown(Key.G))
            return;

        QuickPick.InitializingSelector = true;
        OpenNodeBrowser.Invoke(__instance, [null, null]);
        QuickPick.InitializingSelector = false;
    }
}