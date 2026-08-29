using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSOverworld : Scene
{
    // what we wanna do:
    // - copy vanilla overworld
    // - except add system to cleanly hand full control over to mods' overworlds TODO: should we give full control?
    // - '- other option: use config files + a separate editor app (ewie)
    // - config files for like better detail on how to switch over might be needed
    // - first hand over control to any other specified default or just go with our self
    // - !!! mod overworlds will have to cooperate for us to get control back cleanly
    // - fallback to us first; we can get diagnostics. fallback to vanilla if we fail !
    private MountainRenderer Mountain;
    private HiresSnow Snow;
    public BOSOverworld(OverworldLoader loader)
    {
        Add(Mountain = new MountainRenderer());
        Add(Snow = loader.Snow ?? new HiresSnow());
        RendererList.UpdateLists();
    }
}