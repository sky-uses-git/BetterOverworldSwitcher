using System;
using Celeste.Mod.Helpers;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSCamController : Entity
{
    public Vector3 Target { get; private set; }
    public bool CircleCam = false;
    public float CircleMag = 0f;
    public float CircleRot = 0f;
    public float CircleVdsp = 0f;
    public float CircleRotSpeed = .0015f;
    public MountainCamera Camera;
    public MountainModel ViewerTarget { get; private set; }

    public void GoTo(Vector3 pos)
    {
        Camera.Position = pos;
    }

    public Vector3 GetCamVec(Vector3 rel) => Vector3.Transform(rel, Camera.Rotation.Conjugated());
    public Vector3 GetForward => GetCamVec(Vector3.Forward);
    public Vector3 GetBackward => GetCamVec(Vector3.Backward);
    public Vector3 GetUp => GetCamVec(Vector3.Up);
    public Vector3 GetDown => GetCamVec(Vector3.Down);
    public Vector3 GetLeft => GetCamVec(Vector3.Left);
    public Vector3 GetRight => GetCamVec(Vector3.Right);

    public void Circle(Vector3 pos,Vector3 disp)
    {
        Camera.Position = pos + disp;
        Camera.LookAt(pos);
        Target = pos;
        CircleCam = true;
        CircleMag = disp.Length();
        CircleRot = (float)Math.Atan2(disp.Z,disp.X);
        CircleVdsp = disp.Y;
    }

    public void Circle(Vector3 pos, float howfar=10, float vertdisp=0, float theta = 0)
    {
        float s = (float)Math.Sin(theta);
        float c = (float)Math.Cos(theta);
        Vector3 dispvec = ( Vector3.Forward * c + Vector3.Right * s ) * howfar + Vector3.Up * vertdisp;
        Circle(pos,dispvec);
    }

    public override void Update()
    {
        if (BOSHostScene.Instance.Debug)
        {
            base.Update();
            return;
        }

        if (CircleCam)
        {
            float s = (float)Math.Sin(CircleRot);
            float c = (float)Math.Cos(CircleRot);
            Vector3 dispvec = ( Vector3.Forward * c + Vector3.Right * s ) * CircleMag + Vector3.Up * CircleVdsp;
            Camera.Position = Target+dispvec;
            Camera.LookAt(Target);
            CircleRot += CircleRotSpeed;
        }
        else
        {
        }

        ViewerTarget.Camera = Camera;

        base.Update();
    }

    public BOSCamController(MountainModel Viewer)
    {
        ViewerTarget = Viewer;
        Circle(new Vector3(0,4.3f,1.125f),18,6,(float)Math.PI*.75f);
    }
}