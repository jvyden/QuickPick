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
}