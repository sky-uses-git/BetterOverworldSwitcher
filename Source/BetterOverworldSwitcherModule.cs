using MonoMod.ModInterop;
using System;

namespace Celeste.Mod.BetterOverworldSwitcher;

public class BetterOverworldSwitcherModule : EverestModule {
    public static BetterOverworldSwitcherModule Instance { get; private set; }

    public override Type SettingsType => typeof(BOSSettings);
    public static BOSSettings Settings => (BOSSettings) Instance._Settings;

    public override Type SessionType => typeof(BOSSession);
    public static BOSSession Session => (BOSSession) Instance._Session;

    public override Type SaveDataType => typeof(BOSSaveData);
    public static BOSSaveData SaveData => (BOSSaveData) Instance._SaveData;

    public static readonly string BOSAssetPath = "Overworld/BOS/";

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

    private static void LoadBOSContent()
    {
        Logger.Info("BOS","Loading content");
    }

    public override void Load()
    {
        Everest.Events.GameLoader.OnLoadThread += BOSHooks.HookToOverworldLoader;
        BOSHooks.BeforeOverworldLoaded += LoadBOSContent;
    }

    public override void Unload() {
    }
}