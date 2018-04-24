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

        public bool ToggleRed()
        {
            appData.IsRed = !appData.IsRed;
            return appData.IsRed;
        }

        public void OnGet()
        {
        }

    }
}
