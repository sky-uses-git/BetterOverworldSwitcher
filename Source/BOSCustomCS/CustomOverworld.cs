using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class CustomOverworld : Scene
{
    private MountainRenderer Mountain;
    private HiresSnow Snow;
    public CustomOverworld(OverworldLoader loader)
    {
        Add(Mountain = new MountainRenderer());
        Add(Snow = loader.Snow ?? new HiresSnow());
        RendererList.UpdateLists();
    }
}