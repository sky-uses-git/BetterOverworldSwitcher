using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher;

public static class BOSHooks
{
    public static bool doVanilla;
    public static void HookToOverworldLoader()
    {
        On.Celeste.OverworldLoader.Begin += ((orig, self) =>
        {
            if (!doVanilla) BOSCustomCS.BOSLoaderFuncs.Begin(self);
            else orig(self);
        });
    }
}