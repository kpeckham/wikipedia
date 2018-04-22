using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesWikipedia.Pages
{
    public class ResearchModel : PageModel
    {
        public void OnGet()
        {
        }

        public int CountPages() 
        {
            return 1; 
        }
    }
}
