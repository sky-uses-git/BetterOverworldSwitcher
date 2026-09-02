using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// does nothing but hold objects
public class UiFrame : UiElement
{
    public Color BackgroundColor = Color.Transparent;
    public UiFrame() : base()
    {
    }
    public UiFrame(Vector2 offset, Vector2 scale) : base(offset,scale)
    {
    }
    public UiFrame(Vector2 offset, Vector2 scale, Color bgColor) : this(offset,scale)
    {
        BackgroundColor = bgColor;
    }
    public override void RenderElement()
    {
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        if (BackgroundColor!=Color.Transparent)
            Draw.Rect(RenderPosition,RenderSize.X,RenderSize.Y,BackgroundColor);
        Draw.SpriteBatch.End();
        base.RenderElement();
    }
}