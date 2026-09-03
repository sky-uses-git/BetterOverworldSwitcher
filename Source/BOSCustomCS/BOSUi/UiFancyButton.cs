using System.Collections;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiFancyButton : UiButton
{
    public UiFancyButton(string text) : base(text)
    {
        PositionTween.Duration = .2f;
    }
    public UiFancyButton(string text, Vector2 offset, Vector2 scale) : base(text, offset, scale)
    {
        PositionTween.Duration = .2f;
    }
    public UiFancyButton(string text, float fontsize, Vector2 offset, Vector2 scale) : base(text, fontsize, offset, scale)
    {
        PositionTween.Duration = .2f;
    }

    public override IEnumerator Select(UiElement last)
    {
        Logger.Info("BOS","select called");
        //bump up
        PositionTween.EaseMode = Ease.CubeOut;
        Position += ScaleOffset.FromOffset(0,-8);
        yield return .1f;
        PositionTween.EaseMode = Ease.CubeIn;
        Position += ScaleOffset.FromOffset(0,8);
        yield return .1f;
        yield return base.Select(last);
    }

    public override IEnumerator Deselect(UiElement next)
    {
        Logger.Info("BOS","deselect called");
        yield return base.Deselect(next);
    }
}