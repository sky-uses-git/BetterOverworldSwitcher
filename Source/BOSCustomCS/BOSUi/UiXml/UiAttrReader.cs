using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;

public class UiAttrReader
{
    private static T GetAttr<T>(XmlElement elem,string name,T defaultval)
    {
        if (!elem.HasAttribute(name)) return defaultval;
        string s_val = elem.GetAttribute(name);
        TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
        if (!conv.CanConvertFrom(typeof(string))) {
            Logger.Error("BOS attr","cannot convert type "+typeof(T).Name+" to string ("+name+" = "+s_val+")");
            throw new ArgumentOutOfRangeException(typeof(T).Name);
        }
        if (typeof(T) == typeof(Color)) // special case(ew)
        {
            Color? col = colorFromHex(s_val);
            if (col == null) goto invalid;
            return (T)(object)col;
        }
        if (!conv.IsValid(s_val)) goto invalid;
        return (T)conv.ConvertFromString(s_val);
invalid:
        Logger.Warn("BOS attr","passed value of "+s_val+" for "+name+" (type "+typeof(T).Name+") is invalid");
        return defaultval;
    }

    private static UiAttr.Transform GetTransform(XmlElement elem)
    {
        int offsposx = GetAttr<int>(elem, "offsposx", 0);
        int offsposy = GetAttr<int>(elem, "offsposy", 0);
        float sclposx = GetAttr<float>(elem, "sclposx", 0f);
        float sclposy = GetAttr<float>(elem, "sclposy", 0f);
        int offssizx = GetAttr<int>(elem, "offssizx", 0);
        int offssizy = GetAttr<int>(elem, "offssizy", 0);
        float sclsizx = GetAttr<float>(elem, "sclsizx", 0f);
        float sclsizy = GetAttr<float>(elem, "sclsizy", 0f);
        return new UiAttr.Transform(){
            Position=new ScaleOffset(offsposx, offsposy, sclposx, sclposy),
            Size=new ScaleOffset(offssizx, offssizy, sclsizx, sclsizy)
        };
    }

    private static Color? colorFromHex(string num)
    {
        int bgcolor = 0;
        if (!int.TryParse(num, NumberStyles.HexNumber, null, out bgcolor))
            return null;
        return new Color((bgcolor & 0xFF0000) >> 16, (bgcolor & 0xFF00) >> 8, bgcolor & 0xFF);
    }

    private static UiAttr.Common GetCommon(XmlElement elem)
    {
        UiAttr.Common com = new();
        com.bgColor = GetAttr<Color>(elem, "bgcolor", Color.Transparent);
        com.opacity = GetAttr<float>(elem, "bgopacity", 0f);
        return com;
    }

    private static UiAttr.Text GetTextAttrs(XmlElement elem)
    {
        UiAttr.Text tx = new();
        tx.opacity = GetAttr<float>(elem,"txopacity",1f);
        tx.shadowopacity = GetAttr<float>(elem,"txopacity",.4f);
        tx.size = GetAttr<float>(elem,"fontsize",24f);
        tx.shadowdist = GetAttr<float>(elem,"shdist",8f);
        tx.color = GetAttr<Color>(elem,"txcolor",Color.White);
        tx.shadowcolor = GetAttr<Color>(elem,"shcolor",Color.Black);
        tx.Value = GetAttr<string>(elem,"value","Empty");
        return tx;
    }

    private static string GetId(XmlElement elem)
    {
        return GetAttr<string>(elem,"id",elem.LocalName+"-"+Guid.NewGuid());
    }

    private static UiAttr.Navigation GetNav(XmlElement elem)
    {
        UiAttr.Navigation nav = new();
        nav.up = elem.GetAttribute("up");
        nav.down = elem.GetAttribute("down");
        nav.left = elem.GetAttribute("left");
        nav.right = elem.GetAttribute("right");
        return nav;
    }

    private static UiAttr.Button GetButtonAttrs(XmlElement elem)
    {
        UiAttr.Button btn = new();
        btn.gotoid = GetAttr<string>(elem,"goto",null);
        return btn;
    }

    public static UiAttr.Element ConstructAttrs(XmlElement ui,UiAttr.Element parent,string? id=null)
    {
        UiAttr.Element attrs = new();
        attrs.parent = parent;
        attrs.children = new();
        foreach (XmlNode uiChildNode in ui.ChildNodes)
            if (uiChildNode.GetType().IsAssignableTo(typeof(XmlElement)))
            {
                XmlElement elem = (XmlElement)uiChildNode;
                string childId = GetId(elem);
                attrs.children[childId] = ConstructAttrs(elem,attrs,childId);
            }

        attrs.transform = GetTransform(ui);
        attrs.id = id ?? GetId(ui);
        attrs.nav = GetNav(ui);
        string type = ui.LocalName;
        switch(type) {
            case "Root":
            {
                string selfirst = ui.GetAttribute("select");
                if (selfirst.Length == 0) selfirst = null;
                attrs.root.selectFirst = selfirst;
                break;
            }
            case "TextLabel":
            {
                attrs.common = GetCommon(ui);
                attrs.text = GetTextAttrs(ui);
                break;
            }
            case "Button":
            case "FancyButton":
            {
                attrs.common = GetCommon(ui);
                attrs.text = GetTextAttrs(ui);
                attrs.button = GetButtonAttrs(ui);
                break;
            }
            case "Frame":
            { 
                attrs.common = GetCommon(ui);
                break;
            }
            default: { break; }
        }
        return attrs;
    }
}