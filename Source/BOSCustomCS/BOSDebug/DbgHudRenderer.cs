using Celeste.Mod.Core;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSDebug;

public class DbgHudRenderer : Renderer
{
    private BOSHostScene HostScene => BOSHostScene.Instance;
    private bool debugEnabled => HostScene.Debug;
    
    private DbgCamController camController;
    private BOSCamController HostCamController => HostScene.CamController;

    public DbgHudRenderer()
    {
        HostScene.Add(camController = new DbgCamController());
        HostScene.Entities.UpdateLists();
    }

    public override void Render(Scene scene)
    {
        Draw.SpriteBatch.Begin();
        string st = getDebugString();
        Vector2 meas = Draw.DefaultFont.MeasureString(st);
        Draw.Rect(8, 8, meas.X + 8, meas.Y + 8, Color.Black * 0.5f);
        Draw.Text(Draw.DefaultFont, st, new Vector2(12, 12), Color.White);
        Draw.SpriteBatch.End();
        base.Render(scene);
    }

    public override void Update(Scene scene)
    {
        Visible = debugEnabled;
        base.Update(scene);
    }

    private string getCameraString()
    {
        return "\tPOS: " + HostScene.Renderer.Viewer.Camera.Position + "\n" +
               "\tROT: " + HostScene.Renderer.Viewer.Camera.Rotation +
               "\n" +
               "\tTAR: " + HostScene.Renderer.Viewer.Camera.Target + "\n" +
               "cSPD: " + camController.Speed;
    }

    private string getViewerString()
    {
        if (HostScene.Renderer.Viewer == null) return "VIEWER = NULL";
        return $"camera\n{getCameraString()}";
    }

    private string getDebugString()
    {
        return (
            "hello from "+GetType().Name+"\n"+
            "BetterOverworldSwitcher "+BetterOverworldSwitcherModule.Instance.Metadata.VersionString+"\n"+
            Everest.BuildString + "\n\n" +
            getViewerString() + "\n\n"
            );
    }
}