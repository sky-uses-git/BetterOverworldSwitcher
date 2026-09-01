using Celeste.Mod.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSDebug;

public class DbgCamController : Entity
{
    private MountainModel Viewer => BOSHostScene.Instance.Renderer.Viewer;
    public float Speed = 1f;
    private Vector2 oldPos;

    public DbgCamController()
    {
    }

    public override void Update()
    {
        if (!BOSHostScene.Instance.Debug) { base.Update(); return; }
        
        if (CoreModule.Settings.CameraForward.Check) dbg_moveCamRel(Vector3.Forward,Speed);
        if (CoreModule.Settings.CameraBackward.Check) dbg_moveCamRel(Vector3.Backward,Speed);
        if (CoreModule.Settings.CameraLeft.Check) dbg_moveCamRel(Vector3.Left,Speed);
        if (CoreModule.Settings.CameraRight.Check) dbg_moveCamRel(Vector3.Right,Speed);
        if (CoreModule.Settings.CameraUp.Check) dbg_moveCamRel(Vector3.Up,Speed);
        if (CoreModule.Settings.CameraDown.Check) dbg_moveCamRel(Vector3.Down,Speed);
        if (MInput.Keyboard.Pressed(Keys.L)) dbg_centerCam();
        if (MInput.Keyboard.Pressed(Keys.K)) Speed = 1f;

        if (MInput.Mouse.CheckRightButton)
        {
            int gameCenterX = Engine.Graphics.GraphicsDevice.Viewport.Width/2;
            int gameCenterY = Engine.Graphics.GraphicsDevice.Viewport.Height/2;
            MouseState state = Mouse.GetState();
            int mouseDeltaX = state.X - gameCenterX;
            int mouseDeltaY = state.Y - gameCenterY;
            if (mouseDeltaX != 0 || mouseDeltaY != 0)
                dbg_rotCam(mouseDeltaX,mouseDeltaY);
            Mouse.SetPosition(gameCenterX,gameCenterY);
        }
        else oldPos = -Vector2.One;

        if (MInput.Mouse.WheelDelta != 0)
        {
            if (MInput.Mouse.WheelDelta > 0) Speed *= 1.1f;
            else Speed /= 1.1f;
        }

        base.Update();
    }
    private void dbg_moveCamRel(Vector3 rel,float speed)
    {
        Viewer.Camera.Position += Vector3.Transform(rel, Viewer.Camera.Rotation.Conjugated())*Engine.DeltaTime*speed;
    }
    private void dbg_centerCam()
    {
        Viewer.Camera.Position = Vector3.Zero;
    }

    private void dbg_rotCam(int mouseDeltX,int mouseDeltY)
    {
        Vector3 right = Vector3.Transform(Vector3.Right, Viewer.Camera.Rotation.Conjugated());
        Vector3 up = Vector3.UnitY;
        Quaternion pitch = Quaternion.CreateFromAxisAngle(right, mouseDeltY/100f);
        Quaternion yaw = Quaternion.CreateFromAxisAngle(up, mouseDeltX/100f);
        Viewer.Camera.Rotation *= pitch;
        Viewer.Camera.Rotation *= yaw;
        Viewer.Camera.Rotation = Quaternion.Normalize(Viewer.Camera.Rotation);
    }
}