using System.Collections.Generic;
using System.Numerics;
using System.Xml;
using Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;
using Microsoft.Xna.Framework;
using Monocle;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi;

// TEMPORARY!!!!!!!
// TODO: load from xml files
public class UiLoader
{
    private static XmlLoader xmlLoader = new();

    public UiElement ConstructUIFromXml(XmlElement ui)
    {
        UiAttr.Element attrs = UiAttrReader.ConstructAttrs(ui,new UiAttr.Element(){TopLevel = true});
        
        List<UiElement> Children = new();
        foreach (XmlNode uiChildNode in ui.ChildNodes)
            if (uiChildNode.GetType().IsAssignableTo(typeof(XmlElement)))
                Children.Add(ConstructUIFromXml((XmlElement)uiChildNode));
        UiElement me;
        switch (ui.LocalName)
        {
            case "Root": {
                me = new UiRoot();
                me.Size = ScaleOffset.FromScale(1, 1);
                ((UiRoot)me).SelectFirst = attrs.root.selectFirst;
                break;
            }
            case "Frame": {
                me = new UiFrame(attrs.transform.Position.Offset,attrs.transform.Position.Scale);
                me.Size = attrs.transform.Size;
                ((UiFrame)me).BackgroundColor = attrs.common.bgColor*attrs.common.opacity;
                break;
            }
            case "TextLabel": {
                me = new UiTextLabel(attrs.text.Value,attrs.text.size,attrs.transform.Position.Offset,attrs.transform.Position.Scale);
                me.Size = attrs.transform.Size;
                ((UiTextLabel)me).BackgroundColor = attrs.common.bgColor*attrs.common.opacity;
                break;
            }
            case "FancyButton": {
                me = new UiFancyButton(attrs.text.Value,attrs.text.size,attrs.transform.Position.Offset,attrs.transform.Position.Scale);
                me.Size = attrs.transform.Size;
                ((UiFancyButton)me).BackgroundColor = attrs.common.bgColor*attrs.common.opacity;
                break;
            }
            default: {
                me = new UiTextLabel("unknown elem",16,Vector2.Zero,Vector2.One*.5f);
                ((UiTextLabel)me).BackgroundColor = Color.Black * .5f;
                me.Position = attrs.transform.Position;
                me.Size = attrs.transform.Size;
                break;
            }
        }
        me.id = attrs.id;
        me.UpElement = attrs.nav.up;
        me.DownElement = attrs.nav.down;
        me.LeftElement = attrs.nav.left;
        me.RightElement = attrs.nav.right;
        Children.ForEach(e=>me.AddChild(e));
        return me;
    }

    public UiElement LoadFromXML(string filename) // TODO: add lua support for xml buttons? ?
    {
        XmlElement ui = xmlLoader.Load(filename);
        if (ui != null)
            return ConstructUIFromXml(ui);
        return null;
    }

    public UiRoot Load(string id)
    {
        return (UiRoot)LoadFromXML("Graphics/Atlases/Mountain/SkyIsYou/BetterOverworldSwitcher/Ui/" + id)
               ?? loadnotfound(id);
    }

    private UiRoot loadnotfound(string id)
    {
        UiRoot root = new("notfound");
        root.Size = ScaleOffset.FromScale(1,1);
        UiTextLabel notfoundtx = new("UI of ID "+id+" not found or failed to load", 72,new Vector2(0, -50), new Vector2(0, 0.333f));
        notfoundtx.TextColor = Color.Red;
        notfoundtx.BackgroundColor = Color.Black*.75f;
        notfoundtx.Size = new ScaleOffset(0,100,1,0);
        UiFancyButton backButton = new("Back to root", new Vector2(-200, -40), new Vector2(0.5f, 0.667f));
        backButton.TextColor = Color.White;
        backButton.id = "backbutton";
        backButton.BackgroundColor = Color.Black;
        backButton.Size = ScaleOffset.FromOffset(400, 80);
        backButton.OnPress += () => BOSHudRenderer.Instance.Goto("root");
        root.AddChild(notfoundtx);
        root.AddChild(backButton);
        root.SelectFirst = "backbutton";
        return root;
    }
}