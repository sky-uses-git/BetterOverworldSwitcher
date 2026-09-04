using System.Collections;
using System.Threading;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSLoaderFuncs
{
    // vanilla OverworldLoader loads overworld on separate thread
    // and copying what they do makes it look very clean :D
    
    private static BOSHostScene overworld;
    private static bool loaded;
    private static OverworldLoader refloader;
    private static Thread activeThread;

    public static IEnumerable EnsureDependencies()
    {
        while (!Celeste3DEngineLoader.Loaded) yield return null;
    }

    public static void Begin(OverworldLoader self)
    {
        loaded = false;
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

        Session sesh = null;
        if (SaveData.Instance != null) sesh = SaveData.Instance.CurrentSession_Safe;
        
        Entity handoverEnt = new Entity();
        handoverEnt.Add(new Coroutine(Routine(sesh)));
        self.Add(handoverEnt);
        activeThread = Thread.CurrentThread;
        activeThread.Priority = ThreadPriority.Lowest;
        BOSHooks.InvokeBeforeOverworldLoaded();
        RunThread.Start(LoadThread, "BOS_OVERWORLD_LOADER", highPriority: true);
    }

    private static void LoadThread()
    {
        overworld = new BOSHostScene(refloader);
        overworld.Entities.UpdateLists();
        loaded = true;
        BOSHooks.InvokeAfterOverworldLoaded();
        activeThread.Priority = ThreadPriority.Normal;
    }

    private static IEnumerator Routine(Session session)
    {
        yield return EnsureDependencies();
        while (!loaded) yield return null;
        Engine.Scene = overworld;
    }
}