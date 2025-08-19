using Microsoft.Playwright;
using System.Diagnostics;

namespace TAF_iSAMS.Pages.Utilities
{
    public class FrameManager
    {
        #region variables
        private readonly IPage _page;

        public List<FrameWrapper> iFrames = new List<FrameWrapper>();
        private Dictionary<string, Action<IFrame>> frameMappings;

        private HashSet<string> frameTypes = new HashSet<string>
        {
            "iFrameManager", "main_frame",
            "iFrameContent", "txtFrameContent","pneContent",
            "iFrameTop", "frameTop", "pneTop",
            "txtFrameData","iFrameData",
            "iFrameOptions","iFrameRight","iFrameLeft","toolbar",
            "iFrameSets","editframe","iframe1","listframe",
            "iFrameTimetable","iFrameArrange","Record","iFrameRecord",
            "tr_enquiry","enquiryframe",
            "iframe_iSAMS_CTRLPANEL_USERS",
            "iFrameOptionsTop","dialog-body",
            "iFrameOptionsTop", "iFrameResults", "txtFrameLeft",
            "txtFrameTop", "txtFrameOptions"
        };

        #endregion
        #region Frames
        public IFrame ContainerFrame { get; private set; }
        public IFrame ManagerFrame { get; private set; }
        public IFrame TopFrame { get; private set; }
        public IFrame NestedTopFrame { get; private set; }
        public IFrame ContentFrame { get; private set; }
        public IFrame NestedContentFrame { get; private set; }
        public IFrame OptionsFrame { get; private set; }
        public IFrame DataFrame { get; private set; }
        public IFrame ResultsFrame { get; private set; }
        public IFrame LeftFrame { get; private set; }
        public IFrame RightFrame { get; private set; }
        public IFrame ToolbarFrame { get; private set; }
        public IFrame SetsFrame { get; private set; }
        public IFrame EditFrame { get; private set; }
        public IFrame iFrame1 { get; private set; }
        public IFrame ListFrame { get; private set; }
        public IFrame TimetableFrame { get; private set; }
        public IFrame ArrangeFrame { get; private set; }
        public IFrame RecordFrame { get; private set; }//record
        public IFrame NestedRecordFrame { get; private set; }//iFrameRecord
        public IFrame EnquiryFrame { get; private set; }
        public IFrame DialogBody { get; private set; }
        public IFrame ControlPanelUsersFrame { get; private set; }
        public IFrame txtFrameLeft { get; private set; }
        public IFrame txtFrameTop { get; private set; }
        public IFrame txtFrameOptions { get; private set; }



        #endregion
        #region Methods
        public FrameManager(IPage page)
        {
            _page = page;
        }

        public async Task DetectAndInitialiseFrames(IPage page)
        {
            iFrames.Clear();
            await GetIFramesRecursiveAsync(page.Frames, iFrames);
            await InitialiseStoredFrames();
        }


        private async Task GetIFramesRecursiveAsync(IEnumerable<IFrame> frames, List<FrameWrapper> visibleIFrames)
        {
            foreach (var frame in frames)
            {
                var wrappedFrame = new FrameWrapper(frame);
                await wrappedFrame.InitializeAsync();

                if ((frameTypes.Contains(wrappedFrame.Name) || frameTypes.Contains(wrappedFrame.Id)) && !visibleIFrames.Contains(wrappedFrame))
                {
                    visibleIFrames.Add(wrappedFrame);
                }

                Debug.WriteLine($"Name: {wrappedFrame.Name}, ID: {wrappedFrame.Id}");
                await GetIFramesRecursiveAsync(frame.ChildFrames.ToList(), visibleIFrames);
            }
        }

        private void AssignContentFrame(IFrame frame)
        {
            if (frame.Name == "iFrameTop")
            {
                IsNestedTopFrame(frame);
                return;
            }

            if (IsChildOfContentFrame(frame))
                NestedContentFrame = frame;
            else
                ContentFrame = frame;
        }

        public void IsNestedTopFrame(IFrame frame)//In some modules there is a nested iFrameTop
        {
            if(IsChildOfContentFrame(frame))
                NestedTopFrame = frame;
            else
                TopFrame = frame;
        }
        private bool IsChildOfContentFrame(IFrame frame)//Is the iframe a child of iFrameContent?
        {
            return frame.ParentFrame != null &&
                (frame.ParentFrame.Name == "iFrameContent" || frame.ParentFrame.Name == "txtFrameContent" || frame.ParentFrame.Name == "pneContent");
        }
        public async Task InitialiseStoredFrames()
        {
            frameMappings = new Dictionary<string, Action<IFrame>>
            {
                { "iFrameManager", frame => ManagerFrame = frame },
                { "main_frame", frame => ManagerFrame = frame },
                { "iFrameContent", frame => AssignContentFrame(frame) },
                { "txtFrameContent", frame => AssignContentFrame(frame) },
                { "pneContent", frame => AssignContentFrame(frame) },
                { "iframe1", frame => iFrame1 = frame },
                { "iFrameTop", frame => AssignContentFrame(frame) },
                { "frameTop", frame => TopFrame = frame },
                { "pneTop", frame => TopFrame = frame },
                { "iFrameOptions", frame => OptionsFrame = frame },
                { "iFrameRight", frame => RightFrame = frame },
                { "iFrameLeft", frame => LeftFrame = frame },
                { "toolbar", frame => ToolbarFrame = frame },
                { "iFrameSets", frame => SetsFrame = frame },
                { "editframe", frame => EditFrame = frame },
                //{ "iframe1", frame => OneFrame = frame },
                { "listframe", frame => ListFrame = frame },
                { "iFrameTimetable", frame => TimetableFrame = frame },
                { "iFrameArrange", frame => ArrangeFrame = frame },
                { "Record", frame => RecordFrame = frame },
                { "iFrameRecord", frame => NestedRecordFrame = frame },
                { "enquiryframe", frame => AssignContentFrame(frame) },
                { "tr_enquiry", frame => AssignContentFrame(frame) },
                { "iframe_iSAMS_CTRLPANEL_USERS", frame => ControlPanelUsersFrame = frame },
                { "dialog-body", frame => DialogBody = frame },
                { "iFrameData", frame => DataFrame = frame },
                { "iFrameResults", frame => ResultsFrame = frame },
                { "txtFrameLeft", frame => txtFrameLeft = frame },
                { "txtFrameData", frame => DataFrame = frame },
                { "txtFrameTop", frame => txtFrameTop = frame },
                { "txtFrameOptions", frame => txtFrameOptions = frame }



                // Add more mappings as needed
            };

            foreach (var frame in iFrames)
            {
                if (frameMappings.TryGetValue(frame.Name, out var assignFrame) || frameMappings.TryGetValue(frame.Id, out assignFrame))                
                    assignFrame(frame.Frame);               
            }
        }
        #endregion

        #region OldFunctions
        // Retrieve the module container frame based on the unique module selector.
        //public async Task<IFrame> GetModuleContainerFrameAsync(string moduleSelector)
        //{
        //    Console.WriteLine($"Attempting to get module container frame using selector: {moduleSelector}");
        //    var moduleElement = await _page.WaitForSelectorAsync(moduleSelector);
        //    Console.WriteLine($"Module container frame found.");
        //    return await moduleElement.ContentFrameAsync();
        //}

        //// Gets the main container frame (iFrameManager)
        //public async Task<IFrame> GetManagerFrameAsync(IFrame containerFrame)
        //{
        //    Console.WriteLine($"Attempting to get manager frame.");
        //    // Look for iFrameManager or an equivalent selector within the container.
        //    var managerElement = await containerFrame.WaitForSelectorAsync("#iFrameManager, [name='iFrameManager'], #main_frame");
        //    Console.WriteLine($"Manager frame found.");
        //    return await managerElement.ContentFrameAsync();
        //}

        //// Gets the top navigation frame inside the manager frame (iFrameTop)
        //public async Task<IFrame> GetTopFrameAsync(IFrame managerFrame)
        //{
        //    Console.WriteLine($"Attempting to get top frame.");
        //    var topFrameElement = await managerFrame.WaitForSelectorAsync("#iFrameTop, [name='iFrameTop'], #frameTop, [name='pneTop']");
        //    Console.WriteLine($"Top frame found.");
        //    return await topFrameElement.ContentFrameAsync();
        //}

        //// Gets the options navigation frame inside the content frame (iFrameContent) // Currently does not work
        //public async Task<IFrame> GetOptionsFrameAsync(IFrame contentFrame)
        //{
        //    Console.WriteLine($"Attempting to get content frame.");
        //    var optionsFrameElement = await contentFrame.WaitForSelectorAsync("[name='iFrameOptions']");
        //    Console.WriteLine($"Content frame found.");
        //    return await optionsFrameElement.ContentFrameAsync();
        //}

        #endregion
    }
}
