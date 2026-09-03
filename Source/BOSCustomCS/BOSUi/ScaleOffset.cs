using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public class ScaleOffset
{
    public Vector2 Offset;
    public Vector2 Scale;
    public ScaleOffset(Vector2 offs, Vector2 scl)
    {
        Offset = offs;
        Scale = scl;
    }
    public ScaleOffset(int ox,int oy,float sx,float sy)
    {
        Offset = new Vector2(ox,oy);
        Scale = new Vector2(sx,sy);
    }

    public static ScaleOffset FromOffset(int ox, int oy) => new(ox, oy, 0, 0);
    public static ScaleOffset FromScale(float sx, float sy) => new(0,0,sx, sy);

    public static ScaleOffset Zero => new(0, 0, 0, 0);

    public static ScaleOffset Lerp(ScaleOffset from, ScaleOffset to, float ease) =>
        new(Vector2.Lerp(from.Offset, to.Offset, ease), Vector2.Lerp(from.Scale, to.Scale, ease));

    public static ScaleOffset operator +(ScaleOffset x, ScaleOffset y) => new ScaleOffset(x.Offset + y.Offset, x.Scale + y.Scale);
}