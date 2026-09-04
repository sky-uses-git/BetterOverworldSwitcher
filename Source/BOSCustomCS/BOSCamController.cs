using System;
using Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;
using Celeste.Mod.Celeste3DEngine;
using Celeste.Mod.Helpers;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS;

public class BOSCamController : Entity
{
    private static float f_degtorad(float th) => th * (float)Math.PI / 180;
    public Tweenable<Vector3> _tweenTarget { get; private set; }

    public Vector3 Target
    {
        get => _tweenTarget.Value;
        private set => _tweenTarget.TweenTo(value);
    }
    public Tweenable<Vector3> _tweenCamPosition { get; private set; }

    public Vector3 CamPosition
    {
        get => _tweenCamPosition.Value;
        private set => _tweenCamPosition.TweenTo(value);
    }

    public BOSEnum.CameraMode CamMode = BOSEnum.CameraMode.CircleAroundPos;
    public Tweenable<float> _tweencircleMag { get; private set; }
    public Tweenable<float> _tweencircleRot { get; private set; }
    public Tweenable<float> _tweencircleVdsp { get; private set; }
    private float _circleRotOffset = 0f;
    private bool transitioning => _tweenTarget.Tween.TimeLeft>0f;
    public float CircleMag
    {
        get => _tweencircleMag.Value;
        private set => _tweencircleMag.TweenTo(value);
    }

    public float CircleRot
    {
        get => _tweencircleRot.Value;
        private set => _tweencircleRot.TweenTo(value);
    }
    public float CircleVdsp
    {
        get => _tweencircleVdsp.Value;
        private set => _tweencircleVdsp.TweenTo(value);
    }
    public float CircleRotSpeed { get; private set; } = .0015f;
    public Scene3D ViewerTarget { get; private set; }
    public Camera3D Camera { get; private set; }

    public Vector3 GetCamVec(Vector3 rel) => Vector3.Transform(rel, Camera.transform.Rotation.Conjugated());
    public Vector3 GetForward => GetCamVec(Vector3.Forward);
    public Vector3 GetBackward => GetCamVec(Vector3.Backward);
    public Vector3 GetUp => GetCamVec(Vector3.Up);
    public Vector3 GetDown => GetCamVec(Vector3.Down);
    public Vector3 GetLeft => GetCamVec(Vector3.Left);
    public Vector3 GetRight => GetCamVec(Vector3.Right);

    private void moveAroundTo(Vector3 pos,Vector3 dispvec)
    {
        Target = pos;
        Camera.transform.SetPosition(Target + dispvec);
        Camera.transform.LookAt(Target);
        CircleMag = dispvec.XZ().Length();
        CircleRot = (float)Math.Atan2(dispvec.Z,dispvec.X);
        CircleVdsp = dispvec.Y;
    }

    private void moveAroundTo(Vector3 pos, float howfar=10, float vertdisp=0, float theta = 0, bool degrees=true)
    {
        if (degrees) theta = f_degtorad(theta);
        _circleRotOffset = 0f;
        float s = (float)Math.Sin(theta);
        float c = (float)Math.Cos(theta);
        Vector3 dispvec = ( Vector3.Forward * c + Vector3.Right * s ) * howfar + Vector3.Up * vertdisp;
        moveAroundTo(pos,dispvec);
    }

    public void Jump(Vector3 pos, float howfar = 10, float vertdisp = 0, float theta = 0, bool degrees=true)
    {
        CamMode = BOSEnum.CameraMode.JumpAroundPos;
        moveAroundTo(pos,howfar,vertdisp,theta,degrees);
    }
    public void Circle(Vector3 pos, float howfar = 10, float vertdisp = 0, float theta = 0, bool degrees=true)
    {
        CamMode = BOSEnum.CameraMode.CircleAroundPos;
        moveAroundTo(pos,howfar,vertdisp,theta,degrees);
    }
    public void Move(Vector3 pos, Vector3 target)
    {
        CamMode = BOSEnum.CameraMode.FreePos;
        CamPosition = pos;
        Target = target;
    }

    public override void Update()
    {
        if (BOSHostScene.Instance.Debug)
        {
            // dont do anything (debug cam controller will be enabled)
            base.Update();
            return;
        }

        if (CamMode == BOSEnum.CameraMode.CircleAroundPos || CamMode == BOSEnum.CameraMode.JumpAroundPos)
        {
            float s = (float)Math.Sin(CircleRot+_circleRotOffset);
            float c = (float)Math.Cos(CircleRot+_circleRotOffset);
            Vector3 dispvec = ( Vector3.Forward * c + Vector3.Right * s ) * CircleMag + Vector3.Up * CircleVdsp;
            Camera.transform.SetPosition(Target+dispvec);
            Camera.transform.LookAt(Target);
            if (CamMode == BOSEnum.CameraMode.CircleAroundPos && !transitioning)
                _circleRotOffset += CircleRotSpeed;
        }
        else
        {
            Camera.transform.SetPosition(CamPosition);
            Camera.transform.LookAt(Target);
        }

        base.Update();
    }

    public BOSCamController()
    {
        _tweenTarget = new Tweenable<Vector3>(Vector3.Zero, 1f, Vector3.Lerp);
        _tweencircleMag = new Tweenable<float>(0f, 1f, float.Lerp);
        _tweencircleRot = new Tweenable<float>(0f, 1f, float.Lerp);
        _tweencircleVdsp = new Tweenable<float>(0f, 1f, float.Lerp);
        _tweenTarget.CompleteTweenOnUpdate = true;
        _tweencircleMag.CompleteTweenOnUpdate = false;
        _tweencircleRot.CompleteTweenOnUpdate = false;
        _tweencircleVdsp.CompleteTweenOnUpdate = false;
        Add(_tweenTarget.Tween);
        Add(_tweencircleMag.Tween);
        Add(_tweencircleRot.Tween);
        Add(_tweencircleVdsp.Tween);
    }

    public override void Added(Scene scene)
    {
        ViewerTarget = EngineEntity.GetCurrentScene();
        Camera = ViewerTarget.GetRenderingCamera();
        Circle(new Vector3(0,4.3f,1.125f),18,6,-62.5f);
        _tweenTarget.CompleteTweenOnUpdate = false;
        base.Added(scene);
    }
}