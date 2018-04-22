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

        public void loopPages() {
            Dictionary<int, FirstLinkInfo> ToPhilosophy = new Dictionary<int, FirstLinkInfo>();
            using (var db = new WikiDbContext()) {
                byte[] compareText = Encoding.UTF8.GetBytes("Featured_articles");
                var pages = db.Categorylinks.Where(cl => cl.ClTo == compareText);

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

        public int FindPhilosophy(int FromId) {
            
        }
    }

    public class FirstLinkInfo
    {
        public int Depth;
        public bool GoesToPhilosophy;
        public bool InALoop;
        public int PageId;

        public FirstLinkInfo(int PageId, bool GoesToPhilosophy, bool InALoop, int Depth) 
        {
            this.Depth = Depth;
            this.PageId = PageId;
            this.InALoop = InALoop;
            this.GoesToPhilosophy = GoesToPhilosophy;
        }
    }
}
