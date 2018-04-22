using System;
using System.Collections.Generic;

namespace RazorPagesWikipedia.DbModels
{
    public partial class Categorylinks
    {
        public uint ClFrom { get; set; }
        public byte[] ClTo { get; set; }
        public byte[] ClSortkey { get; set; }
        public DateTimeOffset ClTimestamp { get; set; }
        public byte[] ClSortkeyPrefix { get; set; }
        public byte[] ClCollation { get; set; }
    }
}
