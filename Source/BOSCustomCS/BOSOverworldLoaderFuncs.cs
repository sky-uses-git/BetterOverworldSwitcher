using System.Collections;
using System.Threading;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public static class BOSOverworldLoaderFuncs
{
    // vanilla OverworldLoader loads overworld on separate thread
    // and copying what they do makes it look very clean :D
    
    private static BOSOverworld overworld;
    private static bool loaded;
    private static OverworldLoader refloader;
    private static Thread activeThread;

    public static void Begin(OverworldLoader self)
    {
        // OverworldLoader is not initialized yet
        // and won't go thru vanilla init (bad?)
        refloader = self;
        self.Add(self.Snow);
        if (self.fadeIn)
        {
            ScreenWipe.WipeColor = Color.Black;
            new FadeWipe(self, wipeIn: true);
        }
        self.RendererList.UpdateLists();

        Entity handoverEnt = new Entity();
        handoverEnt.Add(new Coroutine(Routine()));
        self.Add(handoverEnt);
        activeThread = Thread.CurrentThread;
        activeThread.Priority = ThreadPriority.Lowest;
        RunThread.Start(LoadThread, "BOS_OVERWORLD_LOADER", highPriority: true);
    }

    private static void LoadThread()
    {
        overworld = new BOSOverworld(refloader);
        overworld.Entities.UpdateLists();
        loaded = true;
        activeThread.Priority = ThreadPriority.Normal;
    }

    private static IEnumerator Routine()
    {
        while (!loaded) yield return null;
        Engine.Scene = overworld;
    }
}