using Azure;
using Microsoft.Playwright;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TAF_iSAMS.Pages.Utilities;

public static class FrameDebugger
{


    public static async Task<List<DomElement>> GetDomElementsAsync(IFrame frame)
    {
        var jsonElement = await frame.EvaluateAsync<JsonElement>(@"() => {
            const allowedTags = ['input', 'a', 'b', 'div', 'span', 'button', 'label', 'textarea', 'select'];
            return Array.from(document.querySelectorAll('*'))
                .filter(el => allowedTags.includes(el.tagName.toLowerCase()))
                .map(el => ({
                    tag: el.tagName,
                    id: el.id,
                    name: el.name || '',
                    classList: Array.from(el.classList),
                    text: el.textContent?.trim().slice(0, 100) || ''
                }));
        }");

        return JsonSerializer.Deserialize<List<DomElement>>(jsonElement.GetRawText());
    }

    public static async Task DebugAllFramesDomAsync(IPage page)
    {


        var allDomData = new Dictionary<string, List<DomElement>>();

        // Get DOM elements from the main frame (i.e., the page itself)
        var mainDom = await GetDomElementsAsync(page.MainFrame);
        allDomData["Document"] = mainDom; // Neutral label

        // Get DOM elements from each iframe
        foreach (var frame in page.Frames)
        {
            if (frame == page.MainFrame) continue;

            var frameDom = await GetDomElementsAsync(frame);
            var frameLabel = !string.IsNullOrEmpty(frame.Name)
                ? $"Frame: {frame.Name}"
                : $"Frame: {frame.Url}";

            allDomData[frameLabel] = frameDom;
        }

        var jsonOutput = JsonSerializer.Serialize(allDomData, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        TestContext.WriteLine("DOM Elements by Source:");
        TestContext.WriteLine(jsonOutput);

    }


    public static async Task<FoundElementInfo?> FindElementRecursivelyAsync(IPage page, string searchValue)
    {
        string safeId = "found-" + Guid.NewGuid().ToString("N");

        async Task<FoundElementInfo?> SearchFrameAsync(IFrame frame)
        {
            var json = await frame.EvaluateAsync<string>(@"({ searchValue, safeId }) => {
            function normalize(str) {
                return str?.replace(/\s+/g, ' ').trim().toLowerCase();
            }

            const allowedTags = ['input', 'a', 'b', 'div', 'span', 'button', 'label', 'textarea', 'select'];

            document.querySelectorAll(`[data-found-id='${safeId}']`).forEach(el => el.removeAttribute('data-found-id'));

            const target = normalize(searchValue);

            if (target === 'save & close') {
                const candidates = Array.from(document.querySelectorAll('div.button'))
                    .filter(el => normalize(el.textContent || '').includes(target));

                if (candidates.length > 0) {
                    const el = candidates[0];
                    el.setAttribute('data-found-id', safeId);
                    return JSON.stringify({
                        tag: el.tagName,
                        id: el.id || '',
                        name: el.name || '',
                        title: el.title || '',
                        classList: Array.from(el.classList),
                        text: normalize(el.textContent || '')
                    });
                }
                return null;
            }

            const elements = Array.from(document.querySelectorAll('*'))
                .filter(el => allowedTags.includes(el.tagName.toLowerCase()))
                .map(el => ({
                    el: el,
                    tag: el.tagName,
                    id: el.id || '',
                    name: el.name || '',
                    title: el.title || '',
                    classList: Array.from(el.classList),
                    text: normalize(el.textContent || '')
                }));

            const match = elements.find(el =>
                el.id === searchValue ||
                el.name === searchValue ||
                el.title === searchValue ||
                (el.text && el.text.includes(target))
            );

            if (match && match.el) {
                match.el.setAttribute('data-found-id', safeId);
                return JSON.stringify(match);
            }
            return null;
        }", new { searchValue, safeId });

            if (json == null) return null;

            var element = JsonSerializer.Deserialize<DomElement>(json);
            if (element == null) return null;

            string selector = $"[data-found-id='{safeId}']";
            var frameLabel = !string.IsNullOrEmpty(frame.Name) ? frame.Name : frame.Url;

            TestContext.WriteLine("Element Found:");
            TestContext.WriteLine($"Frame: {frameLabel}");
            TestContext.WriteLine($"Tag: {element.Tag}");
            TestContext.WriteLine($"ID: {element.Id}");
            TestContext.WriteLine($"Name: {element.Name}");
            TestContext.WriteLine($"Title: {element.Title}");
            TestContext.WriteLine($"Selector: {selector}");

            return new FoundElementInfo
            {
                Element = element,
                FrameNameOrUrl = frameLabel,
                Selector = selector,
                FrameReference = frame
            };
        }

        var result = await SearchFrameAsync(page.MainFrame);
        if (result != null) return result;

        foreach (var frame in page.Frames)
        {
            if (frame == page.MainFrame) continue;

            result = await SearchFrameAsync(frame);
            if (result != null) return result;
        }

        TestContext.WriteLine("Element not found in any frame.");
        return null;
    }









}
