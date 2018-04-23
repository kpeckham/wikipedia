using System;
using System.Collections.Generic;

namespace RazorPagesWikipedia.DbModels
{
    public partial class Pagelinks
    {
        public uint PlFrom { get; set; }
        public int PlNamespace { get; set; }
        public byte[] PlTitle { get; set; }
        public int PlFromNamespace { get; set; }
    }
}
