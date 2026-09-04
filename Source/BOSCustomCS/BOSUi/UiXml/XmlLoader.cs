using System.IO;
using System.Xml;
namespace Celeste.Mod.BetterOverworldSwitcher.BOSCustomCS.BOSUi.UiXml;

public class XmlLoader
{
    public XmlElement Load(string assetname)
    {
        ModAsset xml = Everest.Content.Get<AssetTypeXml>(assetname);
        if (xml == null)
        {
            Logger.Info("BOS XML","asset name not found");
            return null;
        }

        XmlDocument doc = new XmlDocument();
        XmlReader reader = XmlReader.Create(xml.Stream);
        try
        {
            doc.Load(reader);
        }
        catch (XmlException aeiou)
        {
            Logger.Warn("BOS XML",$"xml error {aeiou.Message}");
            return null;
        }

        return doc.DocumentElement;
    }
}