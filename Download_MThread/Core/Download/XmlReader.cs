using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Download_MThread.Core.Download
{
    public static class XmlReader
    {
        public static List<ImageFile> GetXmlFiles(string fileName)
        {
            var cardsList = new List<ImageFile>();

            var xDoc = new XmlDocument();
            xDoc.Load(fileName);

            var cards = xDoc.GetElementsByTagName("Card");
            for (var i = 0; i < cards.Count; i++)
            {
                var xmlElement = cards[i]["image"];
                if (xmlElement != null)
                {
                    cardsList.Add(MakeImageFileFile(cards[i]));
                }
            }
            return cardsList; 
        }
        private static ImageFile MakeImageFileFile(XmlNode xmlNode)
        {
            var file = new ImageFile();
            if (xmlNode["image"] != null)
            {
                file.Url = xmlNode["image"].InnerText;
            }
            if (xmlNode["name"] != null)
            {
                file.Name = xmlNode["name"].InnerText;
            }
            return file;
        }
    }
}
