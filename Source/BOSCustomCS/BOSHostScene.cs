using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

// what we wanna do:
// - copy vanilla overworld
// - except add system to cleanly hand full control over to mods' overworlds TODO: should we give full control?
// - '- other option: use config files + a separate editor app (ewie)
// - config files for like better detail on how to switch over might be needed
// - first hand over control to any other specified default or just go with our self
// - !!! mod overworlds will have to cooperate for us to get control back cleanly
// - fallback to us first; we can get diagnostics. fallback to vanilla if we fail !

public class BOSHostScene : Scene
{
    public static BOSHostScene Instance { get; private set; }

    public BOSRenderer Renderer;
    public BOSHudRenderer Hud;
    public HiresSnow Snow;
    
    public bool Debug = false;
    
    public override void Update()
    {
        if (MInput.Keyboard.Pressed(Keys.Space)) Instance.Debug = !Instance.Debug;
        base.Update();
    }

    public BOSHostScene(OverworldLoader loader)
    {
        Instance = this;
        Add(Renderer = new BOSRenderer());
        Add(Snow = loader.Snow ?? new HiresSnow());
        Add(Hud = new BOSHudRenderer(loader.StartMode));
        RendererList.UpdateLists();
    }

    public override void End()
    {
        Renderer.End();
        Hud.End();
        Instance = null;
        base.End();
    }

    public void LoadVanilla()
    {
        BOSHooks.forceVanilla = true;
        Engine.Scene = new OverworldLoader(Overworld.StartMode.Titlescreen);
    }
}