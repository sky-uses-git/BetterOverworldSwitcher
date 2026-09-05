using System;
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
    private float campitch = 0f;
    private float camyaw = 0f;
    private bool justPressedRight = false;
    private Vector3? initialPos = Vector3.Zero;

    public DbgCamController()
    {
    }

    public override void Update()
    {
        if (!BOSHostScene.Instance.Debug)
        {
            initialPos = null;
            base.Update(); return;
        }
        if (initialPos == null) // set our debug rot to cam rot when we are activated
        {
            initialPos = Viewer.Camera.Position;
            Vector3 look = initialPos.Value - Viewer.Camera.Target;
            look.Normalize();
            campitch = (float)Math.Atan2(-look.X, look.Z);
            camyaw = (float)Math.Atan2(look.Y, look.XZ().Length());
        }
        
        if (CoreModule.Settings.CameraForward.Check) dbg_moveCamRel(Vector3.Forward,Speed);
        if (CoreModule.Settings.CameraBackward.Check) dbg_moveCamRel(Vector3.Backward,Speed);
        if (CoreModule.Settings.CameraLeft.Check) dbg_moveCamRel(Vector3.Left,Speed);
        if (CoreModule.Settings.CameraRight.Check) dbg_moveCamRel(Vector3.Right,Speed);
        if (CoreModule.Settings.CameraUp.Check) dbg_moveCamRel(Vector3.Up,Speed);
        if (CoreModule.Settings.CameraDown.Check) dbg_moveCamRel(Vector3.Down,Speed);
        if (MInput.Keyboard.Pressed(Keys.L)) dbg_centerCam();
        if (MInput.Keyboard.Pressed(Keys.K)) Speed = 1f;


        if (MInput.Mouse.WheelDelta != 0)
        {
            if (MInput.Mouse.WheelDelta > 0) Speed *= 1.1f;
            else Speed /= 1.1f;
        }
        
        if (MInput.Mouse.CheckRightButton)
        {

            int gameCenterX = Engine.Graphics.GraphicsDevice.Viewport.Width / 2;
            int gameCenterY = Engine.Graphics.GraphicsDevice.Viewport.Height / 2;
            MouseState state = Mouse.GetState();
            int mouseDeltaX = state.X - gameCenterX;
            int mouseDeltaY = state.Y - gameCenterY;
            Mouse.SetPosition(gameCenterX, gameCenterY);
            if (!justPressedRight) { // drop first frame of mouse input intentionally so we dont go to last mouse pos
                justPressedRight = true;
                base.Update();
                return;
            }
            if (mouseDeltaX != 0 || mouseDeltaY != 0)
                dbg_rotCam(mouseDeltaX, mouseDeltaY);
        }
        else
        {
            justPressedRight = false;
            oldPos = -Vector2.One;
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
        campitch += mouseDeltX/100f;
        camyaw += mouseDeltY/100f;
        if (Math.Abs(camyaw) > Math.PI / 2) camyaw = Math.Sign(camyaw) * ((float)Math.PI / 2);
        if (campitch > Math.Tau || campitch < 0) campitch %= (float)Math.Tau;
        Quaternion pitch = Quaternion.CreateFromAxisAngle(Vector3.Up, campitch);
        Quaternion yaw = Quaternion.CreateFromAxisAngle(Vector3.Right, camyaw);
        Viewer.Camera.Rotation = Quaternion.Identity * yaw * pitch;
    }
}