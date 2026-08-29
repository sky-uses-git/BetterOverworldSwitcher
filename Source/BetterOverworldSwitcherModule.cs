using System;

namespace Celeste.Mod.BetterOverworldSwitcher;

public class BetterOverworldSwitcherModule : EverestModule {
    public static BetterOverworldSwitcherModule Instance { get; private set; }

    public override Type SettingsType => typeof(BetterOverworldSwitcherModuleSettings);
    public static BetterOverworldSwitcherModuleSettings Settings => (BetterOverworldSwitcherModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(BetterOverworldSwitcherModuleSession);
    public static BetterOverworldSwitcherModuleSession Session => (BetterOverworldSwitcherModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(BetterOverworldSwitcherModuleSaveData);
    public static BetterOverworldSwitcherModuleSaveData SaveData => (BetterOverworldSwitcherModuleSaveData) Instance._SaveData;

    public BetterOverworldSwitcherModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(BetterOverworldSwitcherModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(BetterOverworldSwitcherModule), LogLevel.Info);
#endif
    }

    public override void Load() {
        // TODO: apply any hooks that should always be active
    }

    public override void Unload() {
        // TODO: unapply any hooks applied in Load()
    }
}