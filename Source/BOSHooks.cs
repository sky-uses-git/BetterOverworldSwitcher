using System;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher;

public static class BOSHooks
{
    public static bool forceVanilla = false;
    public static event Action BeforeOverworldLoaded;
    public static event Action AfterOverworldLoaded;

    public static void InvokeBeforeOverworldLoaded() => BeforeOverworldLoaded?.Invoke();
    public static void InvokeAfterOverworldLoaded() => AfterOverworldLoaded?.Invoke();
    public static void HookToOverworldLoader()
    {
        On.Celeste.OverworldLoader.Begin += ((orig, self) =>
        {
            if (!BetterOverworldSwitcherModule.Settings.ToggleHost) forceVanilla = true;
            if (forceVanilla) orig(self);
            else BOSCustomCS.BOSLoaderFuncs.Begin(self);
        });
    }
}