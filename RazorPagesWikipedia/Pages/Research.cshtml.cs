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
        public MyAppData appData;

        public ResearchModel(MyAppData appData)
        {
            this.appData = appData;
        }



        public void OnGet()
        {
        }

    }
}
