using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAF_iSAMS.Pages.Utilities
{

    public class ElementResult
    {
        public IElementHandle? Handle { get; set; }
        public string? JsSelector { get; set; } // Only set if fallback was used
        public IFrame? Frame { get; set; } // Needed to execute JS in correct context

    }

}
