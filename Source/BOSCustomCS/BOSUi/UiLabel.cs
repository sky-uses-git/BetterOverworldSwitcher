using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiLabel : UiFrame
{
    public string Value = "Label";
    public Color TextColor = Color.White;
    public float FontSize = 24;
    private float RealFontSize => (FontSize / ActiveFont.FontSize.Size);
    public UiEnum.TextAlign TextAlign = UiEnum.TextAlign.Center;
    public UiLabel(string text, Vector2 position) : base(position)
    {
        Value = text;
    }
    public UiLabel(string text, Vector2 position, Vector2 offs) : base(position, offs)
    {
        Value = text;
    }
    public UiLabel(string text, Vector2 position, Vector2 offs, Color bgColor) : base(position, offs, bgColor)
    {
        Value = text;
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

    public override void Render()
    {
        base.Render();
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        Vector2 txoffs = alignoffsets[(int)TextAlign];
        Vector2 txsize = ActiveFont.FontSize.Measure(Value)*ScaleFactor*RealFontSize;
        Vector2 addpos = (RealSize - txsize)*txoffs;
        ActiveFont.Draw(Value,RealPosition+addpos,Vector2.Zero,ScaleFactor * RealFontSize,TextColor);
        Draw.SpriteBatch.End();
    }
}