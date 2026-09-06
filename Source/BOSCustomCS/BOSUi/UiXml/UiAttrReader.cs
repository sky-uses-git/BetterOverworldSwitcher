using System;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;

public class UiAttrReader
{
    private static UiAttr.Transform GetTransform(XmlElement elem)
    {
        string s_offsposx = elem.GetAttribute("offsposx");
        string s_offsposy = elem.GetAttribute("offsposy");
        string s_sclposx = elem.GetAttribute("sclposx");
        string s_sclposy = elem.GetAttribute("sclposy");
        string s_offssizx = elem.GetAttribute("offssizx");
        string s_offssizy = elem.GetAttribute("offssizy");
        string s_sclsizx = elem.GetAttribute("sclsizx");
        string s_sclsizy = elem.GetAttribute("sclsizy");
        int offsposx=0;
        int offsposy=0;
        float sclposx=0;
        float sclposy=0;
        int offssizx=0;
        int offssizy=0;
        float sclsizx=0;
        float sclsizy=0;
        if ((s_offsposx.Length==0 || int.TryParse(s_offsposx, out offsposx)) &&
            (s_offsposy.Length==0 || int.TryParse(s_offsposy, out offsposy)) &&
            (s_sclposx.Length==0 || float.TryParse(s_sclposx, out sclposx)) &&
            (s_sclposy.Length==0 || float.TryParse(s_sclposy, out sclposy)) &&
            (s_offssizx.Length==0 || int.TryParse(s_offssizx, out offssizx)) &&
            (s_offssizy.Length==0 || int.TryParse(s_offssizy, out offssizy)) &&
            (s_sclsizx.Length==0 || float.TryParse(s_sclsizx, out sclsizx)) &&
            (s_sclsizy.Length==0 || float.TryParse(s_sclsizy, out sclsizy)))
            return new UiAttr.Transform(){
                Position=new ScaleOffset(offsposx, offsposy, sclposx, sclposy),
                Size=new ScaleOffset(offssizx, offssizy, sclsizx, sclsizy)
            };
        return new UiAttr.Transform();
    }

    private static Color colorFromHex(string num)
    {
        int bgcolor = 0;
        if (!int.TryParse(num, NumberStyles.HexNumber, null, out bgcolor))
            return Color.Transparent;
        return new Color((bgcolor & 0xFF0000) >> 16, (bgcolor & 0xFF00) >> 8, bgcolor & 0xFF);
    }

    private static UiAttr.Common GetCommon(XmlElement elem)
    {
        UiAttr.Common com = new();
        string s_bgcolor = elem.GetAttribute("bgcolor");
        string s_opacity = elem.GetAttribute("bgopacity");
        Color bgcolor = colorFromHex(s_bgcolor);
        float opacity = 0f;
        if (s_opacity.Length != 0) float.TryParse(s_opacity, out opacity);
        com.bgColor = bgcolor;
        com.opacity = opacity;
        return com;
    }

    private static UiAttr.Text GetTextAttrs(XmlElement elem)
    {
        UiAttr.Text tx = new();
        string s_txcolor = elem.GetAttribute("txcolor");
        string s_shcolor = elem.GetAttribute("shcolor");
        string s_txopacity = elem.GetAttribute("txopacity");
        string s_shopacity = elem.GetAttribute("shopacity");
        string s_txsize = elem.GetAttribute("fontsize");
        string s_shdist = elem.GetAttribute("shdist");
        string txvalue = elem.GetAttribute("value");
        Color txcolor = colorFromHex(s_txcolor);
        Color shcolor = colorFromHex(s_shcolor);
        float txopacity = 1;
        float shopacity = .4f;
        float txsize = 24;
        float shdist = 8;
        if (s_shdist.Length != 0) float.TryParse(s_txopacity, out txopacity);
        if (s_shdist.Length != 0) float.TryParse(s_shopacity, out shopacity);
        if (s_txsize.Length != 0) float.TryParse(s_txsize, out txsize);
        if (s_shdist.Length != 0) float.TryParse(s_shdist, out shdist);
        tx.opacity = txopacity;
        tx.shadowopacity = shopacity;
        tx.size = txsize;
        tx.shadowdist = shdist;
        tx.color = txcolor;
        tx.shadowcolor = shcolor;
        tx.Value = txvalue;
        return tx;
    }

    private static string GetId(XmlElement elem)
    {
        string id = elem.GetAttribute("id");
        if (id.Length == 0) return elem.LocalName+"-"+Guid.NewGuid();
        return id;
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
            case "Button": { 
                attrs.common = GetCommon(ui);
                attrs.text = GetTextAttrs(ui);
                break; }
            case "FancyButton":
            {
                attrs.common = GetCommon(ui);
                attrs.text = GetTextAttrs(ui);
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