using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;
using VirtualButton = On.Monocle.VirtualButton;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiButton : UiFrame
{
    public string Value = "Button";
    public Color TextColor = Color.White;
    public Color SelectColor = Color.White;

    public UiEnum.TextAlign TextAlign = UiEnum.TextAlign.Center;
    public UiButton(string text, Vector2 position) : base(position)
    {
        Value = text;
    }
    public UiButton(string text, Vector2 position, Vector2 offs) : base(position, offs)
    {
        Value = text;
    }
    public UiButton(string text, Vector2 position, Vector2 offs, Color bgColor) : base(position, offs, bgColor)
    {
        Value = text;
        BackgroundColor = bgColor;
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
        Color oldbkg = BackgroundColor;
        Color oldtxt = TextColor;
        if (Selected) BackgroundColor = SelectColor;
        if (Selected) TextColor = Color.Black;
        base.Render();
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        Vector2 txoffs = alignoffsets[(int)TextAlign];
        Vector2 txsize = Draw.DefaultFont.MeasureString(Value);
        Vector2 addpos = (Size - txsize)*txoffs;
        Draw.Text(Draw.DefaultFont,Value,RealPosition+addpos,TextColor);
        Draw.SpriteBatch.End();
        BackgroundColor = oldbkg;
        TextColor = oldtxt;
    }
}