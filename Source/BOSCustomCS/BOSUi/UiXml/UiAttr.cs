using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;

public static class UiAttr
{
    public struct Transform
    {
        public ScaleOffset Position;
        public ScaleOffset Size;
    }

    public struct Element
    {
        public bool TopLevel;
        public Dictionary<string, Element> children;
        public object? parent;
        public Transform transform;
        public Common common;
        public Text text;
        public Root root;
    }

    public struct Common
    {
        public Color bgColor;
        public float opacity;
    }

    public struct Text
    {
        public string Value;
        
        public float size;
        public Color color;
        public float opacity;
        
        public float shadowdist;
        public Color shadowcolor;
        public float shadowopacity;
    }

    public struct Root
    {
        public string selectFirst;
    }
}