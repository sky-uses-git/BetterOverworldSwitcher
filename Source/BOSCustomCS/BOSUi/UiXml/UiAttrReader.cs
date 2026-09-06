using System.Xml;

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
        if ((s_offsposx.Equals("") || int.TryParse(s_offsposx, out offsposx)) &&
            (s_offsposy.Equals("") || int.TryParse(s_offsposy, out offsposy)) &&
            (s_sclposx.Equals("") || float.TryParse(s_sclposx, out sclposx)) &&
            (s_sclposy.Equals("") || float.TryParse(s_sclposy, out sclposy)) &&
            (s_offssizx.Equals("") || int.TryParse(s_offssizx, out offssizx)) &&
            (s_offssizy.Equals("") || int.TryParse(s_offssizy, out offssizy)) &&
            (s_sclsizx.Equals("") || float.TryParse(s_sclsizx, out sclsizx)) &&
            (s_sclsizy.Equals("") || float.TryParse(s_sclsizy, out sclsizy)))
            return new UiAttr.Transform(){
                Position=new ScaleOffset(offsposx, offsposy, sclposx, sclposy),
                Size=new ScaleOffset(offssizx, offssizy, sclsizx, sclsizy)
            };
        return new UiAttr.Transform();
    }
    
    public static UiAttr.Element ConstructAttrs(XmlElement ui,UiAttr.Element parent)
    {
        UiAttr.Element attrs = new();
        attrs.parent = parent;
        foreach (XmlNode uiChildNode in ui.ChildNodes)
            if (uiChildNode.GetType().IsAssignableTo(typeof(XmlElement)))
            {
                XmlElement elem = (XmlElement)uiChildNode;
                string id = elem.GetAttribute("id");
                attrs.children[id] = ConstructAttrs(elem,attrs);
            }

        attrs.transform = GetTransform(ui);
        string type = ui.LocalName;
        switch(type) {
            case "Root": { break; }
            case "Frame": { break; }
            case "TextLabel": { break; }
            case "Button": { break; }
            case "FancyButton": { break; }
            default: { break; }
        }
        return attrs;
    }
}