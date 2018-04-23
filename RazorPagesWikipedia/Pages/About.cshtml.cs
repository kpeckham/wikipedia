using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using RazorPagesWikipedia.DbModels;
using System.Text;

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
            XElement root = XElement.Load("/mnt/volume-nyc3-03/ToPhilosophyFeaturedPages.xml");

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

        public int getNumOfLoops() {
            //LoadToPhilosophy must have already run;
            using (var db = new WikiDbContext())
            {
                byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
                var links = db.Categorylinks.Where(cl => cl.ClTo == CompareText);

                int loopNum = 0;
                //foreach (var id in db.KpFirstlinks.Select(link => 
                foreach (var id in links.Select(link => link.ClFrom))
                {

                    bool inLoop = ToPhilosophy[(int)id].InALoop;
                    if (inLoop) {
                        loopNum++;
                    }
                }
                return loopNum;
            }

        }

        public Tuple<int,double> getNumToPhilosophy()
        {
            //LoadToPhilosophy must have already run;
            using (var db = new WikiDbContext())
            {
                byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
                var links = db.Categorylinks.Where(cl => cl.ClTo == CompareText);

                int philNum = 0;
                double depthCount = 0;
                //foreach (var id in db.KpFirstlinks.Select(link => 
                foreach (var id in links.Select(link => link.ClFrom))
                {

                    bool toPhilosophy = ToPhilosophy[(int)id].GoesToPhilosophy;
                    if (toPhilosophy)
                    {
                        philNum++;
                        depthCount += ToPhilosophy[(int)id].Depth;
                    }
                }
                return new Tuple<int,double>(philNum,depthCount/philNum);
            }

        }

        public int getNumUnProcessed()
        {
            //LoadToPhilosophy must have already run;
            using (var db = new WikiDbContext())
            {
                byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
                var links = db.Categorylinks.Where(cl => cl.ClTo == CompareText);

                int procNum = 0;
                //foreach (var id in db.KpFirstlinks.Select(link => 
                foreach (var id in links.Select(link => link.ClFrom))
                {

                    bool unProcessed = ToPhilosophy[(int)id].unProcessed;
                    if (unProcessed)
                    {
                        procNum++;
                    }
                }
                return procNum;
            }

        }
    }
}
