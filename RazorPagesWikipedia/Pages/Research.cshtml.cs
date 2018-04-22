using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesWikipedia.DbModels;

namespace RazorPagesWikipedia.Pages
{
    public class ResearchModel : PageModel
    {
        public void OnGet()
        {
        }

        public int CountPages() 
        {
            using (var db = new WikiDbContext()) {
                return db.Page.Count();
                // can give count a lambda or could do .Select().Count()
            }

        }
    }
}
