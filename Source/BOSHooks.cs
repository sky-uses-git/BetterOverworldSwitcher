using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher;

public static class BOSHooks
{
    public static void HookToOverworldLoader()
    {
        On.Celeste.OverworldLoader.Begin += ((orig, self) => BOSCustomCS.BOSLoaderFuncs.Begin(self));
    }
}