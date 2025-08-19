using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAF_iSAMS.Pages.Utilities
{
    public class FrameWrapper
    {
        public IFrame Frame { get; }
        public string Id { get; private set; }
        public string Name { get; set; }

        public FrameWrapper(IFrame frame)
        {
            Frame = frame;
        }


        /// <summary>
        /// Returns an IEnumerable<Frame> object containing the child iFrames of the variable 'Frame'
        /// </summary>
        public IEnumerable<IFrame> ChildFrames => Frame.ChildFrames;

        /// <summary>
        /// This function will initialise a given iFrame by using it to obtain the ID attribute and setting the public variable 'Id' to the found value and the 'Name' variable to the frame name.
        /// This allows us to have access to both Frame.Name and Frame.Id.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            try
            {
                var elementHandle = await Frame.FrameElementAsync();
                if (elementHandle != null)
                {
                    Id = await elementHandle.GetAttributeAsync("id");
                    Name = Frame.Name;
                }
            }
            catch (PlaywrightException ex) when (ex.Message.Contains("Frame has been detached"))
            {
                // Handle the detached frame scenario
                Id = null;
            }
        }
    }




}
