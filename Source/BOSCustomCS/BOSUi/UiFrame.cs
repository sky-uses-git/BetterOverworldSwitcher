using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monocle;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// does nothing but hold objects
public class UiFrame : UiElement
{
    public Color BackgroundColor = Color.Transparent;
    public UiFrame(Vector2 position) : base(position)
    {
    }
    public UiFrame(Vector2 position, Vector2 offs) : base(position, offs)
    {
    }
    public UiFrame(Vector2 position, Vector2 offs, Color bgColor) : this(position, offs)
    {
        BackgroundColor = bgColor;
    }
    public override void Render()
    {
        Draw.SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.NonPremultiplied);
        if (BackgroundColor!=Color.Transparent)
            Draw.Rect(RealPosition,RealSize.X,RealSize.Y,BackgroundColor);
        Draw.SpriteBatch.End();
        base.Render();
    }
}