using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class UiButton : UiTextLabel
{
    public Color SelectColor = Color.White;

    public UiEnum.TextAlign TextAlign = UiEnum.TextAlign.Center;
    public UiButton(string text) : base(text)
    {
    }
    public UiButton(string text, Vector2 offset, Vector2 scale) : base(text, offset, scale)
    {
    }
    public UiButton(string text, float fontsize, Vector2 offset, Vector2 scale) : base(text, fontsize, offset, scale)
    {
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
        Color oldbkg = BackgroundColor;
        Color oldtxt = TextColor;
        if (Selected) BackgroundColor = SelectColor;
        if (Selected) TextColor = Color.Black;
        base.RenderElement();
        BackgroundColor = oldbkg;
        TextColor = oldtxt;
    }
}