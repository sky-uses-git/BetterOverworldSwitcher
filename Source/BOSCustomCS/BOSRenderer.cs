using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSRenderer : Renderer
{
    public MountainModel Viewer;

    public BOSRenderer()
    {
        Viewer = new MountainModel();
    }

    public override void BeforeRender(Scene scene)
    {
        Viewer.BeforeRender(scene);
        base.BeforeRender(scene);
    }

    public override void Render(Scene scene)
    {
        Viewer.Render();
        base.Render(scene);
    }
    
    public override void Update(Scene scene)
    {
        Viewer.Update();
        base.Render(scene);
    }

    public void End()
    {
        Viewer.Dispose();
    }
}