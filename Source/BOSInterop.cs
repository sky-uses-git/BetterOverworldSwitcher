using System;
using MonoMod.ModInterop;

namespace Celeste.Mod.BetterOverworldSwitcher;

[ModExportName("BetterOverworldSwitcher")]
public static class BOSInterop
{
    public static event Action RegisterMod;
    public static event Action RegisterCustomOverworldType;
}