using System;
using System.Collections.Generic;

namespace RazorPagesWikipedia.DbModels
{
    public partial class KpFirstlinks
    {
        public uint PageId { get; set; }
        public byte PageIsRedirect { get; set; }
        public byte[] DestinationTitle { get; set; }
    }
}
