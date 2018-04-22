using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesWikipedia.DbModels;
using System.Text;

namespace RazorPagesWikipedia.Pages
{
    public class ResearchModel : PageModel
    {
        Dictionary<int, FirstLinkInfo> ToPhilosophy = new Dictionary<int, FirstLinkInfo>();

        public void OnGet()
        {
        }

        public int CountPages()
        {
            using (var db = new WikiDbContext())
            {
                return db.Page.Count();

                // can give count a lambda or could do .Select().Count()
            }

        }

        public void loopPages()
        {
            using (var db = new WikiDbContext())
            {
                byte[] CompareText = Encoding.UTF8.GetBytes("Featured_articles");
                var pages = db.Categorylinks.Where(cl => cl.ClTo.SequenceEqual(CompareText));

                foreach (var page in pages)
                {
                    if (!ToPhilosophy.ContainsKey((int)page.ClFrom))
                    {
                        ToPhilosophy.Add((int)page.ClFrom, null);
                        FindPhilosophy((int)page.ClFrom);
                    }
                    else
                    {
                        FirstLinkInfo info = ToPhilosophy.GetValueOrDefault((int)page.ClFrom);

                    }
                }
            }
        }

        public FirstLinkInfo FindPhilosophy(int FromId)
        {
            var entry = ToPhilosophy.GetValueOrDefault(FromId);
            if (entry != null)
            {
                if (entry.unProcessed || entry.InALoop)
                {
                    entry.InALoop = true;
                    entry.unProcessed = false;
                }

                return entry;

            }


            using (var db = new WikiDbContext())
            {
                var ToTitleBinary = db.KpFirstlinks.Where(fl => fl.PageId == FromId).Select(fl => fl.DestinationTitle).FirstOrDefault();

                if (ToTitleBinary == null)
                {
                    FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                    ToPhilosophy.Add(FromId, nullEntry);

                    return nullEntry;
                }

                string ToTitle = Encoding.UTF8.GetString(ToTitleBinary);
                var ToId = db.Page.Where(pg => pg.PageTitle.SequenceEqual(ToTitleBinary)).Select(pg => pg.PageId).FirstOrDefault();


                if (ToId == 0)
                {
                    FirstLinkInfo nullEntry = new FirstLinkInfo(0, false, false, -1);
                    ToPhilosophy.Add(FromId, nullEntry);

                    return nullEntry;
                }

                FirstLinkInfo childInfo = ToPhilosophy.GetValueOrDefault((int)ToId);

                if (childInfo != null) {
                    FirstLinkInfo parentInfo = new FirstLinkInfo((int) ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);
                    ToPhilosophy.Add(FromId, parentInfo);
                    return parentInfo;
                }

                if (ToTitle == "Philosophy")
                {
                    FirstLinkInfo info = new FirstLinkInfo((int)ToId, true, false, 1);
                    ToPhilosophy.Add(FromId, info);
                    return info;
                }

                childInfo = FindPhilosophy((int)ToId);
                FirstLinkInfo parentInfo = new FirstLinkInfo((int)ToId, childInfo.GoesToPhilosophy, childInfo.InALoop, childInfo.Depth + 1);
                return parentInfo;
                


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
