using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RazorPagesWikipedia.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }

public Dictionary<int, FirstLinkInfo> ToPhilosophy = new Dictionary<int, FirstLinkInfo>();

public void LoadToPhilosophy()
{
    XElement root = XElement.Load("/mnt/volume-nyc3-03/ToPhilosophy.xml");

    foreach (var el in root.Elements())
    {
        ToPhilosophy.Add(
            Convert.ToInt32(el.Name.LocalName.Substring(1)),
            XmlConvertBack(el.Value)
        );
    }
}

public static FirstLinkInfo XmlConvertBack(string xml)
{
    using (var stringReader = new System.IO.StringReader(xml))
    {
        var serializer = new XmlSerializer(typeof(FirstLinkInfo));
        return serializer.Deserialize(stringReader) as FirstLinkInfo;
    }
}
    }
}
