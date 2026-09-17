using System;
using System.IO;
using System.Xml;
namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;

public class XmlLoader
{
    //todo dont hardcode this
    public static readonly string defaultPath = "Graphics/Atlases/Mountain/SkyIsYou/BetterOverworldSwitcher/Ui/";
    //                                                               <   elem   ,  parent  >
    public static void OperAll(XmlElement elem, string tagName, Action<XmlElement,XmlElement> func)
    {
        foreach (XmlElement o in elem)
        {
            if (o.Name == tagName) func(o,elem);
            OperAll(o,tagName,func);
        }
    }

    public XmlElement Load(string assetname,int depth=-1,string dir=null)
    {
        dir ??= defaultPath;
        depth++;
        ModAsset xml = Everest.Content.Get<AssetTypeXml>(Path.Join(dir,assetname));
        if (xml == null)
        {
            Logger.Info("BOS XML","asset name not found");
            return null;
        }

        XmlDocument doc = new XmlDocument();
        using (XmlReader reader = XmlReader.Create(xml.Stream))
        {
            try
            {
                doc.Load(reader);
            }
            catch (XmlException aeiou)
            {
                Logger.Warn("BOS XML", $"xml error {aeiou.Message}");
                return null;
            }
        }

        //include
        OperAll(doc.DocumentElement, "Include", (incl,parent) =>
        {
            if (depth >= 20) {
                parent.RemoveChild(incl);
                Logger.Warn("BOS XML","Stopping imports due to recursion limit(20)");
                return;
            }
            if (incl.HasAttribute("path"))
            {
                string path = incl.GetAttribute("path");
                string file=Path.GetFileName(path);
                string filedir = Path.Join(dir, Path.GetDirectoryName(path)??"");
                Logger.Info("BOS XML",assetname??"non");
                Logger.Info("BOS XML",path??"non");
                Logger.Info("BOS XML",dir??"non");
                Logger.Info("BOS XML",file??"non");
                Logger.Info("BOS XML",filedir??"non");
                XmlElement loaded = Load(file,depth,filedir);
                if (loaded!=null) {
                    XmlNode import = parent.OwnerDocument.ImportNode(loaded, true);
                    parent.InsertBefore(import,incl);
                }
            }
            parent.RemoveChild(incl);
        });
        
        return doc.DocumentElement;
    }
}