using System;
using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher;

public class Celeste3DEngineLoader
{
    private static readonly EverestModuleMetadata c3dengine_meta = new()
    {
        Name = "Celeste3DEngine",
        Version = new Version(1, 1, 0)
    };
    public static bool Loaded => Everest.Loader.DependencyLoaded(c3dengine_meta);
}