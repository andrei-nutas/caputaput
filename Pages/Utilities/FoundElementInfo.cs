using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAF_iSAMS.Pages.Utilities
{
    public class FoundElementInfo
    {
        public DomElement Element { get; set; }
        public string FrameNameOrUrl { get; set; }
        public string Selector { get; set; }
        public IFrame FrameReference { get; set; } // Optional: for direct use in Playwright
    }

}
