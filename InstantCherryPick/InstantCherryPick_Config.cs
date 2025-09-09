using ResoniteModLoader;

namespace InstantCherryPick;

public partial class InstantCherryPick // Config
{
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> Enabled = new("Enabled", "When checked, enables InstantCherryPick", () => true);
    
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> FreecamFocus = new("FreecamFocus", "When checked, focus the spawned window with the free camera.", () => true);
    
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> TextboxFocus = new("TextboxFocus", "When checked, focus CherryPick's textbox, immediately passing input to CherryPick.", () => true);
}