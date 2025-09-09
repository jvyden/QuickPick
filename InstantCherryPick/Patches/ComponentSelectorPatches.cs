using System.Diagnostics.CodeAnalysis;
using FrooxEngine;
using HarmonyLib;

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
        if (!InstantCherryPick.InitializingSelector) return;
        InstantCherryPick.GeneratedSelector = __instance;
    }
}