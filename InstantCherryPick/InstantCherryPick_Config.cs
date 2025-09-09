using ResoniteModLoader;

namespace InstantCherryPick;

public partial class InstantCherryPick // Config
{
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> Enabled = new("Enabled", "When checked, enables InstantCherryPick", () => true);
    
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> FocusUi = new("FocusUI", "When checked, zoom onto the spawned window.", () => true);
    
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> TextboxFocus = new("TextboxFocus", "When checked, focus CherryPick's textbox, immediately passing input to CherryPick.", () => true);
    
    [AutoRegisterConfigKey]
    public static readonly ModConfigurationKey<bool> DestroySearch = new("DestroySearch", "When checked, the node browser will be destroyed upon picking a node.", () => true);
}