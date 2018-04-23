using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesWikipedia.DbModels;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RazorPagesWikipedia.Pages
{
    public class ResearchModel : PageModel
    {
        public ResearchModel(MyAppData appData)
        {
            this.appData = appData;
        }

        MyAppData appData;

        Dictionary<int, FirstLinkInfo> ToPhilosophy = new Dictionary<int, FirstLinkInfo>();
        public List<int> depths = new List<int>();

        public bool ToggleRed()
        {
            appData.IsRed = !appData.IsRed;
            return appData.IsRed;
        }

        public void OnGet()
        {
        }

        //public Dictionary<int, FirstLinkInfo>()
        //{
        //    using (var db = new WikiDbContext())
        //    {
        //        return db.Page.Count();

        //        // can give count a lambda or could do .Select().Count()
        //    }

        //}

        public void loopPages()
        {
            using (var db = new WikiDbContext())
            {
                byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
                var links = db.Categorylinks.Where(cl => cl.ClTo == CompareText);

                int count = 0;
                //foreach (var id in db.KpFirstlinks.Select(link => link.PageId))
                foreach (var id in links.Select(link => link.ClFrom))
                {
                    if (++count % 1000 == 0)
                        Console.WriteLine(count);
                    FindPhilosophy((int)id);
                }

                XElement root = new XElement("Root",  
                    from keyValue in ToPhilosophy  
                    select new XElement("A" + keyValue.Key.ToString(), XmlConvert(keyValue.Value)));  

                root.Save("/mnt/volume-nyc3-03/ToPhilosophyFeaturedPages.xml");
            }
        }

        public string XmlConvert(FirstLinkInfo item)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(item.GetType());
                serializer.Serialize(stringwriter, item);
                return stringwriter.ToString();
            }
        }

        public FirstLinkInfo FindPhilosophy(int FromId)
        {
            var entry = ToPhilosophy.GetValueOrDefault(FromId);
            if (entry != null)
            {
                Console.WriteLine("We've seen it before");
                if (entry.unProcessed || entry.InALoop)
                {
                    entry.InALoop = true;
                    Console.WriteLine("we've put it in a loop");
                    entry.unProcessed = false;
                }

                return entry;

            }

            FirstLinkInfo unProcessedEntry = new FirstLinkInfo();
            ToPhilosophy.Add(FromId, unProcessedEntry);


            using (var db = new WikiDbContext())
            {
                var ToTitleBinary = db.KpFirstlinks.Where(fl => fl.PageId == FromId).Select(fl => fl.DestinationTitle).FirstOrDefault();

                if (ToTitleBinary == null)
                {
                    FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                    ToPhilosophy[FromId] = nullEntry;

                    return nullEntry;
                }

                string ToTitle = Encoding.UTF8.GetString(ToTitleBinary);
                var ToId = db.Page.Where(pg => pg.PageNamespace == 0 && pg.PageTitle == ToTitleBinary).Select(pg => pg.PageId).FirstOrDefault();


                if (ToId == 0)
                {
                    FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                    ToPhilosophy[FromId] = nullEntry;

                    return nullEntry;
                }

                FirstLinkInfo childInfo = ToPhilosophy.GetValueOrDefault((int)ToId);

                if (childInfo != null) {
                    FirstLinkInfo parentInfo = new FirstLinkInfo((int) ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);
                    if (childInfo.GoesToPhilosophy) 
                    {
                        depths.Add(childInfo.Depth + 1);

                    }

                    ToPhilosophy[FromId] = parentInfo;
                    return parentInfo;
                }

                if (ToTitle == "Philosophy")
                {
                    FirstLinkInfo info = new FirstLinkInfo((int)ToId, true, false, 1);
                    ToPhilosophy[FromId] = info;
                    depths.Add(1);
                    return info;
                }

                childInfo = FindPhilosophy((int)ToId);
                if (childInfo.GoesToPhilosophy) 
                {
                    depths.Add(childInfo.Depth + 1);

                }
                FirstLinkInfo legalGuardianInfo = new FirstLinkInfo((int)ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);
                ToPhilosophy[FromId] = legalGuardianInfo;
                return legalGuardianInfo;
                


            }

        }

    }

    public class FirstLinkInfo
    {
        public int Depth;
        public bool GoesToPhilosophy;
        public bool InALoop;
        public int ToId;
        public bool unProcessed;

        public FirstLinkInfo(int ToId, bool GoesToPhilosophy, bool InALoop, int Depth) 
        {
            this.Depth = Depth;
            this.ToId = ToId;
            this.InALoop = InALoop;
            this.GoesToPhilosophy = GoesToPhilosophy;
            unProcessed = false;
        }

        public FirstLinkInfo ()
        {
            unProcessed = true;
            Depth = -1;
            GoesToPhilosophy = false;
            InALoop = false;
            ToId = 0;

        }
    }
}
