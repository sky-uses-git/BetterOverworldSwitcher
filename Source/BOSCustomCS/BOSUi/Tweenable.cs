using System;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class Tweenable<T>
{
    public T Value { get; private set; }
    private T from;
    private T to;
    public bool Active { get; private set; }

    private readonly Tween tween;
    public Tween Tween => tween;
    private Ease.Easer _easeMode = Ease.CubeOut;
    public bool CompleteTweenOnUpdate = true;

    public float Duration
    {
        get => tween.Duration;
        set => tween.Duration = value;
    }

    public Ease.Easer EaseMode
    {
        get => _easeMode;
        set
        {
            _easeMode = value;
            if (tween != null) tween.Easer = _easeMode;
        }
    }
    public void TweenTo(T target,float duration,bool completeTween)
    {
        Logger.Info("BOS tween","tweento called");
        from = completeTween ? to : Value;
        to = target;
        tween.Duration = duration;
        tween.Start();
    }

    public void TweenTo(T target,bool completeTween) => TweenTo(target,tween.Duration,completeTween);
    public void TweenTo(T target, float duration) => TweenTo(target, duration, CompleteTweenOnUpdate);
    public void TweenTo(T target) => TweenTo(target,tween.Duration,CompleteTweenOnUpdate);

    public Tweenable(T init,float duration, Func<T, T, float, T> lerpFunc)
    {
        Value = init;
        from = init;
        to = init;
        Active = true;
        tween = Tween.Create(Tween.TweenMode.Persist,EaseMode,duration,start: false);
        tween.OnStart = t => Active = true;
        tween.OnUpdate = t => Value = lerpFunc(from, to, t.Eased);
        tween.OnComplete = t => Active = false;
    }

    public void Remove()
    {
        tween?.RemoveSelf();
    }
}