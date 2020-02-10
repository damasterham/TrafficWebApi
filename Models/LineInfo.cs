using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrafikWebAPI.Models
{
    public class LineInfo : BaseModel
    {
        // Would probably define lines somewhere as possibly and enum or list of strings
        // For validation as well as possible efficiency
        public string Line { get; set; }
        // There should probably more validation and such for DateTime since
        // working with time is notoriously difficuly
        // But keeping it simple for this example
        public DateTime Time { get; set; }

        public string Message { get; set; }
    }
}
