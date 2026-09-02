using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

public static class TweenFuncs
{
    public static Color TweenColor(Color from, Color to, float ease) => Color.Lerp(from, to, ease);
    public static Vector2 TweenVec2(Vector2 from, Vector2 to, float ease) => Vector2.Lerp(from, to, ease);
}