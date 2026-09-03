using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiTextLabel : UiFrame
{
    public string Value = "Label";
    public Color TextColor = Color.White;
    public Color StrokeColor = Color.Transparent;
    public Color ShadowColor = Color.Black*.4f;
    public float Stroke = 1f;
    public float ShadowGap = .1f;
    public float FontSize = 24;
    private Vector2 RealFontSize => Vector2.One * (FontSize / ActiveFont.FontSize.Size) * ScaleFactor;
    public UiEnum.TextAlign TextAlign = UiEnum.TextAlign.Center;
    public UiTextLabel(string text)
    {
        Value = text;
    }
    public UiTextLabel(string text, Vector2 offset, Vector2 scale) : base(offset,scale)
    {
        Value = text;
    }
    public UiTextLabel(string text, float fontSize, Vector2 offset, Vector2 scale) : base(offset,scale)
    {
        Value = text;
        FontSize = fontSize;
    }

    private Vector2[] alignoffsets = new[]
    {
        new Vector2(0, 0),
        new Vector2(.5f, 0),
        new Vector2(1, 0),
        new Vector2(0, .5f),
        new Vector2(.5f, .5f),
        new Vector2(1, .5f),
        new Vector2(0, 1),
        new Vector2(.5f, 1),
        new Vector2(1, 1)
    };

    public override void RenderElement()
    {
        base.RenderElement();
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        Vector2 txoffs = alignoffsets[(int)TextAlign];
        Vector2 txsize = ActiveFont.FontSize.Measure(Value)*RealFontSize;
        Vector2 addpos = (RenderSize - txsize)*txoffs;
        if (ShadowColor!=Color.Transparent)
            ActiveFont.Draw(Value,RenderPosition+addpos,new Vector2(0,-ShadowGap),RealFontSize,ShadowColor);
        if (StrokeColor!=Color.Transparent)
            ActiveFont.DrawOutline(Value,RenderPosition+addpos,Vector2.Zero,RealFontSize,TextColor,Stroke,StrokeColor);
        else
            ActiveFont.Draw(Value,RenderPosition+addpos,Vector2.Zero,RealFontSize,TextColor);
        Draw.SpriteBatch.End();
    }
}