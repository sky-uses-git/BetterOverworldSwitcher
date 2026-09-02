namespace Celeste.Mod.BetterOverworldSwitcher;

public class BOSSettings : EverestModuleSettings
{
    [SettingNeedsRelaunch]
    public bool ToggleHost { get; set; } = false;
}