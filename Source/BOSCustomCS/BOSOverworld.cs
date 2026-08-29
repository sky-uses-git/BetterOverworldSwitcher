using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSOverworld : Scene
{
    private MountainRenderer Mountain;
    private HiresSnow Snow;
    public BOSOverworld(OverworldLoader loader)
    {
        Add(Mountain = new MountainRenderer());
        Add(Snow = loader.Snow ?? new HiresSnow());
        RendererList.UpdateLists();
    }
}