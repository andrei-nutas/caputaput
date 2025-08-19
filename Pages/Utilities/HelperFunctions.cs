using Microsoft.Playwright;
using System.Text.Json;

namespace TAF_iSAMS.Pages.Utilities
{
    public static class HelperFunctions
    {

        public static async Task NavigateModuleTabs(IFrame topFrame, string linkText)
        {
            try
            {
                TestContext.WriteLine($"Navigating to module tab with link text: {linkText}");

                // Click a link with the given text in the navigation bar
                await topFrame.Locator($"text={linkText}").ClickAsync();
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"Error navigating to module tab with link text {linkText}: {ex.Message}");
            }
        }

        private static string GetCleanSearchValue(string locator)
        {
            return ExtractSearchValue.ExtractAndReturnSearchValue(locator);
        }

        /// <summary>
        /// This function gets and returns an IElementHandle to interact with.
        /// </summary>
        /// <param name="frame">The iframe to search within</param>
        /// <param name="locator">The locator of the element you would like to search for</param>
        /// <returns></returns>
        public static async Task<ElementResult> GetElement(string locator, IFrame? frame = null, IPage? page = null)
        {
            var isPage = page != null;
            var contextName = isPage ? "page" : $"frame: {frame?.Name ?? "unknown"}";
            TestContext.WriteLine($"Getting Element: {locator} in {contextName}");

            IElementHandle? element = null;

            try
            {
                TestContext.WriteLine($"Trying Playwright WaitForSelector with locator: '{locator}'");
                element = isPage
                    ? await page.WaitForSelectorAsync(locator, new() { Timeout = 3000 })
                    : await frame?.WaitForSelectorAsync(locator, new() { Timeout = 3000 });

                if (element != null)
                {
                    TestContext.WriteLine($"✅ Successfully Got Element: {locator} in {contextName}");
                    return new ElementResult { Handle = element, Frame = frame ?? page?.MainFrame };
                }
            }
            catch (TimeoutException ex)
            {
                TestContext.WriteLine($"⏱️ Timeout getting element {locator}: {ex.Message}");
            }
            catch (PlaywrightException ex)
            {
                TestContext.WriteLine($"⚠️ Playwright error getting element {locator}: {ex.Message}");
            }

            TestContext.WriteLine($"🔍 Primary selector failed. Trying JS fallback for: {locator}");

            var searchValue = ExtractSearchValue.ExtractAndReturnSearchValue(locator)?.Trim();
            TestContext.WriteLine($"Extracted search value for fallback: '{searchValue}'");

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                TestContext.WriteLine("❌ Extracted search value is empty or null.");
                return new ElementResult();
            }

            var pageContext = page ?? frame?.Page;
            if (pageContext == null)
            {
                TestContext.WriteLine("❌ No valid page context for recursive fallback.");
                return new ElementResult();
            }

            TestContext.WriteLine($"Calling FrameDebugger.FindElementRecursivelyAsync with searchValue: '{searchValue}'");
            var fallbackInfo = await FrameDebugger.FindElementRecursivelyAsync(pageContext, searchValue);

            if (fallbackInfo == null)
            {
                TestContext.WriteLine("❌ FrameDebugger.FindElementRecursivelyAsync returned null.");
            }
            else
            {
                TestContext.WriteLine("✅ FrameDebugger.FindElementRecursivelyAsync returned:");
                TestContext.WriteLine($"    Frame: {fallbackInfo.FrameReference?.Name ?? "null"}");
                TestContext.WriteLine($"    Tag: {fallbackInfo.Element?.Tag ?? "null"}");
                TestContext.WriteLine($"    ID: {fallbackInfo.Element?.Id ?? "null"}");
                TestContext.WriteLine($"    Name: {fallbackInfo.Element?.Name ?? "null"}");
                TestContext.WriteLine($"    Title: {fallbackInfo.Element?.Title ?? "null"}");
                TestContext.WriteLine($"    Selector: {fallbackInfo.Selector ?? "null"}");
            }

            if (fallbackInfo != null && !string.IsNullOrEmpty(fallbackInfo.Selector) && fallbackInfo.FrameReference != null)
            {
                TestContext.WriteLine($"✅ JS Fallback succeeded: document.querySelector('{fallbackInfo.Selector}') in frame '{fallbackInfo.FrameReference.Name}'");
                return new ElementResult
                {
                    JsSelector = fallbackInfo.Selector,
                    Frame = fallbackInfo.FrameReference
                };
            }

            TestContext.WriteLine($"❌ JS Fallback failed for: {locator}");
            return new ElementResult();
        }









        /// <summary>
        /// This function clicks an element in a given iFrame
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="locator">The locator of the element you would like to click</param>
        /// <returns></returns>
        public static async Task ClickElementAsync(string locator, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var result = await GetElement(locator, frame, page);

                if (result.Handle != null)
                {
                    TestContext.WriteLine($"🖱️ Clicking Element via Playwright: {locator}");
                    await result.Handle.ClickAsync();
                    return;
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    var executionContext = result.Frame ?? frame ?? page?.MainFrame;
                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid execution context for JS fallback: {locator}");
                        return;
                    }

                    TestContext.WriteLine($"📍 JS fallback executing in frame: {executionContext.Name ?? "unknown frame"}");
                    TestContext.WriteLine($"🖱️ JS fallback selector: {result.JsSelector}");

                    var selectorEscaped = JsonSerializer.Serialize(result.JsSelector);
                    string js;

                    if (locator.Contains("Save & Close", StringComparison.OrdinalIgnoreCase))
                    {
                        // Special handling for Save & Close buttons
                        js = $@"
                            (() => {{
                                const el = document.querySelector({selectorEscaped});
                                if (!el) return false;

                                try {{
                                    el.click(); // Attempt normal click first
                                }} catch (e) {{
                                    console.warn('Standard click failed:', e);
                                }}

                                const inlineOnClick = el.getAttribute('onclick');
                                if (inlineOnClick && typeof inlineOnClick === 'string') {{
                                    try {{
                                        console.log('Executing inline onclick:', inlineOnClick);
                                        eval(inlineOnClick);
                                    }} catch (e) {{
                                        console.warn('Error executing inline onclick:', e);
                                    }}
                                }}
                                return true;
                            }})()
                            ";
                    }
                    else
                    {
                        // Default behavior (keep original checkbox working as-is)
                        js = $@"
                            (() => {{
                                const el = document.querySelector({selectorEscaped});
                                if (!el) return false;

                                // If this element has a checkbox input inside, click the checkbox directly (original working logic)
                                const checkbox = el.querySelector('input[type=checkbox]');
                                if (checkbox) {{
                                    checkbox.click();
                                    return true;
                                }}

                                // Otherwise fallback to synthetic click events
                                ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                    el.dispatchEvent(new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }}));
                                }});
                                return true;
                            }})()
                            ";
                    }

                    await Task.Delay(100);
                    var clicked = await executionContext.EvaluateAsync<bool>(js);

                    TestContext.WriteLine(
                        clicked
                            ? $"✅ JS fallback clicked element: {locator}"
                            : $"❌ JS fallback could not find or click element: {locator}"
                    );
                }
                else
                {
                    TestContext.WriteLine($"❌ Element not found via Playwright or JS fallback: {locator}");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error clicking element {locator}: {ex.Message}");
            }
        }


















        /// <summary>
        /// This function hovers over an element in a given iFrame
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="locator">The locator of the element you would like to click</param>
        /// <returns></returns>
        public static async Task HoverOnElementAsync(string locator, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var result = await GetElement(locator, frame, page);

                if (result.Handle != null)
                {
                    TestContext.WriteLine($"🖱️ Hovering over Element via Playwright: {locator}");
                    await result.Handle.HoverAsync();
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    var executionContext = result.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid frame or page context to evaluate JS for: {locator}");
                        return;
                    }

                    // Debugging frame context
                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"🖱️ JS fallback hover will run in frame/page: {frameInfo}");

                    var selectorEscaped = JsonSerializer.Serialize(result.JsSelector);

                    var jsHoverScript = $@"
                (() => {{
                    try {{
                        const el = document.querySelector({selectorEscaped});
                        if (el) {{
                            ['mouseover', 'mouseenter', 'mousemove'].forEach(type => {{
                                const evt = new MouseEvent(type, {{
                                    bubbles: true,
                                    cancelable: true,
                                    view: window
                                }});
                                el.dispatchEvent(evt);
                                console.log(`Dispatched hover event: ${{type}}`);
                            }});
                            return true;
                        }} else {{
                            console.warn('Element not found for JS fallback hover');
                            return false;
                        }}
                    }} catch (e) {{
                        console.error('JS hover error:', e);
                        return false;
                    }}
                }})()
            ";

                    TestContext.WriteLine($"🖱️ Hovering over Element via JS fallback in {(executionContext == page?.MainFrame ? "page" : "frame")}: document.querySelector({selectorEscaped})");

                    await Task.Delay(100);

                    var hovered = await executionContext.EvaluateAsync<bool>(jsHoverScript);

                    if (!hovered)
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to hover element for selector: {result.JsSelector}");
                    }

                    await Task.Delay(100);
                }
                else
                {
                    TestContext.WriteLine($"❌ Unable to hover over Element: {locator}, no valid selector or JS path");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error hovering over element {locator}: {ex.Message}");
            }
        }



        /// <summary>
        /// This function will select an option inside of a drop down in a given iFrame.
        /// </summary>
        /// <param name="frame">The iFrame to search within</param>
        /// <param name="locator">The locator of the drop down you would like use</param>
        /// <param name="option">The option you would like to select inside of the drop down locator</param>
        /// <returns></returns>
        public static async Task SelectDropdownOptionAsync(string locator, string option, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var result = await GetElement(locator, frame, page);

                if (result.Handle != null)
                {
                    TestContext.WriteLine($"✅ Selecting '{option}' from dropdown via Playwright: {locator}");
                    await result.Handle.SelectOptionAsync(new[] { new SelectOptionValue { Label = option } });
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    var executionContext = result.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid frame or page context to evaluate JS for: {locator}");
                        return;
                    }

                    // Debug context information
                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"🧠 JS fallback will run in frame/page: {frameInfo}");

                    var selectorEscaped = JsonSerializer.Serialize(result.JsSelector);
                    var optionEscaped = JsonSerializer.Serialize(option);

                    var jsSelect = $@"
                (() => {{
                    try {{
                        const dropdown = document.querySelector({selectorEscaped});
                        if (!dropdown) {{
                            console.warn('Dropdown not found for selector: {selectorEscaped}');
                            return false;
                        }}

                        const options = Array.from(dropdown.options);
                        const match = options.find(opt => opt.label === {optionEscaped});
                        if (match) {{
                            dropdown.value = match.value;
                            dropdown.dispatchEvent(new Event('change', {{ bubbles: true }}));
                            console.log('Dropdown option selected and change event dispatched.');
                            return true;
                        }} else {{
                            console.warn('No matching option found with label:', {optionEscaped});
                            return false;
                        }}
                    }} catch (e) {{
                        console.error('JS fallback error selecting dropdown:', e);
                        return false;
                    }}
                }})()
            ";

                    TestContext.WriteLine($"🧠 Selecting '{option}' via JS fallback in {(executionContext == page?.MainFrame ? "page" : "frame")}: document.querySelector({selectorEscaped})");

                    await Task.Delay(100);

                    var selected = await executionContext.EvaluateAsync<bool>(jsSelect);

                    if (!selected)
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to select option '{option}' for selector: {result.JsSelector}");
                    }

                    await Task.Delay(100);
                }
                else
                {
                    TestContext.WriteLine($"❌ Dropdown Element: {locator} not found via Playwright or JS fallback.");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error selecting option '{option}' from dropdown {locator}: {ex.Message}");
            }
        }




        /// <summary>
        /// This function will update a text box in a given iFrame
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="locator">The locator of the text box</param>
        /// <param name="updatedText">The text you would like to insert into text box</param>
        /// <returns></returns>

        public static async Task UpdateTextElementAsync(string locator, string updatedText, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var result = await GetElement(locator, frame, page);

                if (result.Handle != null)
                {
                    TestContext.WriteLine($"⌨️ Updating text element via Playwright: {locator} with text: {updatedText}");
                    await result.Handle.FillAsync(updatedText);
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    var executionContext = result.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid frame or page context to evaluate JS for: {locator}");
                        return;
                    }

                    // Log frame/page info
                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"⌨️ JS fallback will run in frame/page: {frameInfo}");

                    var selectorEscaped = JsonSerializer.Serialize(result.JsSelector);
                    var textEscaped = JsonSerializer.Serialize(updatedText);

                    var jsUpdate = $@"
                (() => {{
                    try {{
                        const el = document.querySelector({selectorEscaped});
                        if (el) {{
                            el.value = {textEscaped};
                            el.dispatchEvent(new Event('input', {{ bubbles: true }}));
                            el.dispatchEvent(new Event('change', {{ bubbles: true }}));
                            console.log('Text updated via JS fallback.');
                            return true;
                        }} else {{
                            console.warn('Element not found for text update.');
                            return false;
                        }}
                    }} catch (e) {{
                        console.error('JS fallback error updating text:', e);
                        return false;
                    }}
                }})()
            ";

                    TestContext.WriteLine($"⌨️ Updating text via JS fallback in {(executionContext == page?.MainFrame ? "page" : "frame")}: document.querySelector({selectorEscaped})");

                    await Task.Delay(100);

                    var updated = await executionContext.EvaluateAsync<bool>(jsUpdate);

                    if (!updated)
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to update text for selector: {result.JsSelector}");
                    }

                    await Task.Delay(100);
                }
                else
                {
                    TestContext.WriteLine($"❌ Unable to update text element: {locator}, no valid selector or fallback path found.");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error updating text element {locator} with text {updatedText}: {ex.Message}");
            }
        }



        /// <summary>
        /// This function will click a link inside of a row in a given iFrame.
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="rowText">The title or text of the row you would like to search for the link within</param>
        /// <param name="linkText">The text of the link you would like to click</param>
        /// <returns></returns>
        public static async Task ClickElementInRow(string rowText, string linkText, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Getting row with text: {rowText} in {contextName}");

                var rowSelector = $"tr:has(td:has-text('{rowText}'))";
                var rowResult = await GetElement(rowSelector, frame, page);

                if (rowResult.Handle != null)
                {
                    TestContext.WriteLine($"✅ Found row with text: {rowText}");

                    var link = await rowResult.Handle.QuerySelectorAsync($"a:has-text('{linkText}')");

                    if (link != null)
                    {
                        TestContext.WriteLine($"✅ Found link with text: {linkText} inside the row");
                        await link.ClickAsync();
                        TestContext.WriteLine($"🖱️ Clicked link with text: {linkText} inside row with text: {rowText}");
                        return;
                    }

                    TestContext.WriteLine($"❌ Could not find link with text: {linkText} inside the row");
                }
                else if (!string.IsNullOrEmpty(rowResult.JsSelector))
                {
                    TestContext.WriteLine($"⚠️ Row not found via Playwright. Using JS fallback: {rowResult.JsSelector}");

                    var executionContext = rowResult.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid execution context found for JS fallback.");
                        return;
                    }

                    // Log frame/page info
                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"🖱️ JS fallback will run in frame/page: {frameInfo}");

                    var jsRowSelector = JsonSerializer.Serialize(rowResult.JsSelector);
                    var jsLinkText = JsonSerializer.Serialize(linkText);

                    var jsClickInRow = $@"
                (() => {{
                    try {{
                        const row = document.querySelector({jsRowSelector});
                        if (!row) return false;

                        const links = Array.from(row.querySelectorAll('a'));
                        const link = links.find(a => a.textContent.trim() === {jsLinkText});
                        if (!link) return false;

                        ['mousedown', 'mouseup', 'click'].forEach(type => {{
                            const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                            link.dispatchEvent(evt);
                        }});

                        return true;
                    }} catch (e) {{
                        console.error('JS error clicking link in row:', e);
                        return false;
                    }}
                }})()
            ";

                    TestContext.WriteLine($"🖱️ Clicking link with text: {linkText} via JS fallback in row with text: {rowText}");

                    var success = await executionContext.EvaluateAsync<bool>(jsClickInRow);

                    if (success)
                    {
                        TestContext.WriteLine($"✅ JS fallback succeeded clicking link: {linkText} in row: {rowText}");
                    }
                    else
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to click link: {linkText} in row: {rowText}");
                    }
                }
                else
                {
                    TestContext.WriteLine($"❌ Failed to find row with text: {rowText} in {contextName} via Playwright or JS fallback.");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error clicking link with text '{linkText}' in row with text '{rowText}': {ex.Message}");
            }
        }



        /// <summary>
        /// This function will click a link inside of a row in a given iFrame.
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="rowText">The title or text of the row you would like to search for the link within</param>
        /// <param name="linkText">The text of the link you would like to click</param>
        /// <returns></returns>
        public static async Task ClickEmptyElementInRow(string rowText, string cellText, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Locating row with text: '{rowText}' in {contextName}");

                string rowSelector = $"tr:has(td:has-text('{rowText}'))";
                var rowResult = await GetElement(rowSelector, frame, page);

                if (rowResult.Handle != null)
                {
                    TestContext.WriteLine($"✅ Row found. Searching for cell with text: '{cellText}'");

                    var cellLocator = await rowResult.Handle.QuerySelectorAsync($"td:has-text('{cellText}')");

                    if (cellLocator != null)
                    {
                        TestContext.WriteLine($"🖱️ Clicking cell with text: '{cellText}' in row '{rowText}'");
                        await cellLocator.ClickAsync();
                        return;
                    }

                    TestContext.WriteLine($"⚠️ Cell with text: '{cellText}' not found in row '{rowText}'");
                }
                else if (!string.IsNullOrEmpty(rowResult.JsSelector))
                {
                    TestContext.WriteLine($"🔍 Row not found via Playwright. Attempting JS fallback: {rowResult.JsSelector}");

                    var executionContext = rowResult.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid frame or page context for JS fallback");
                        return;
                    }

                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"🧠 JS fallback running in: {frameInfo}");

                    var jsClick = $@"
                        (() => {{
                            const row = document.querySelector({JsonSerializer.Serialize(rowResult.JsSelector)});
                            if (row) {{
                                const cell = Array.from(row.querySelectorAll('td')).find(td => td.textContent.trim() === '{cellText}');
                                if (cell) {{
                                    ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                        const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                                        cell.dispatchEvent(evt);
                                    }});
                                    return true;
                                }}
                            }}
                            return false;
                        }})()";

                    var clicked = await executionContext.EvaluateAsync<bool>(jsClick);

                    if (clicked)
                    {
                        TestContext.WriteLine($"✅ JS fallback clicked cell with text: '{cellText}' in row: '{rowText}'");
                    }
                    else
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to find or click cell with text: '{cellText}' in row: '{rowText}'");
                    }
                }
                else
                {
                    TestContext.WriteLine($"❌ Could not find row with text: '{rowText}' via Playwright or JS fallback.");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error clicking cell '{cellText}' in row '{rowText}': {ex.Message}");
            }
        }




        /// <summary>
        /// This function will click a link inside of a row in a given iFrame.
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="rowText">The title or text of the row you would like to search for the link within</param>
        /// <param name="linkText">The text of the link you would like to click</param>
        /// <returns></returns>
        public static async Task ClickLocatorInRow(string rowText, string buttonLocator, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Searching for row with text: '{rowText}' in {contextName}");

                var rowSelector = $"tr:has(td:has-text('{rowText}'))";
                var rowResult = await GetElement(rowSelector, frame, page);

                if (rowResult.Handle != null)
                {
                    TestContext.WriteLine($"✅ Found row with text: '{rowText}'");

                    var target = await rowResult.Handle.QuerySelectorAsync(buttonLocator);

                    if (target != null)
                    {
                        TestContext.WriteLine($"🖱️ Clicking element '{buttonLocator}' inside row '{rowText}' via Playwright");
                        await target.ClickAsync();
                        return;
                    }

                    TestContext.WriteLine($"⚠️ Could not find element '{buttonLocator}' inside row '{rowText}' via Playwright");
                }

                // JS Fallback
                if (!string.IsNullOrEmpty(rowResult.JsSelector))
                {
                    TestContext.WriteLine($"🔁 Attempting JS fallback for element '{buttonLocator}' in row: '{rowText}'");

                    var executionContext = rowResult.Frame ?? frame ?? page?.MainFrame;

                    if (executionContext == null)
                    {
                        TestContext.WriteLine($"❌ No valid execution context (frame/page) available for JS fallback.");
                        return;
                    }

                    string frameInfo;
                    try
                    {
                        frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                    }
                    catch
                    {
                        frameInfo = executionContext.Name ?? "<no name>";
                    }

                    TestContext.WriteLine($"🧠 JS fallback executing in: {frameInfo}");

                    var jsClick = $@"
                        (() => {{
                            const row = document.querySelector({JsonSerializer.Serialize(rowResult.JsSelector)});
                            if (row) {{
                                const target = row.querySelector('{buttonLocator}');
                                if (target) {{
                                    ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                        const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                                        target.dispatchEvent(evt);
                                    }});
                                    return true;
                                }}
                            }}
                            return false;
                        }})()";

                    var clicked = await executionContext.EvaluateAsync<bool>(jsClick);

                    if (clicked)
                    {
                        TestContext.WriteLine($"✅ JS fallback clicked '{buttonLocator}' in row '{rowText}'");
                    }
                    else
                    {
                        TestContext.WriteLine($"❌ JS fallback failed to find or click '{buttonLocator}' in row '{rowText}'");
                    }
                }
                else
                {
                    TestContext.WriteLine($"❌ Could not find row with text '{rowText}' via Playwright or JS fallback.");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"❌ Error clicking locator '{buttonLocator}' in row '{rowText}': {ex.Message}");
            }
        }


        /// <summary>
        /// Clicks an icon in a given row based on it's img src
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="rowText">The title or text of the row you would like to search for the icon within - this is based on an EXACT match</param>
        /// <param name="iconSRC">The img src= text of the icon.</param>
        /// <returns></returns>
        /// 


        public static async Task ClickIconInRow(string rowText, string iconSRC, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var context = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Looking for row with exact cell match: '{rowText}' in {context}");

                var rows = page != null
                    ? await page.QuerySelectorAllAsync("tr")
                    : await frame?.QuerySelectorAllAsync("tr");

                if (rows == null || rows.Count == 0)
                {
                    TestContext.WriteLine("❌ No rows found.");
                    return;
                }

                foreach (var row in rows)
                {
                    var cells = await row.QuerySelectorAllAsync("td");
                    if (cells == null || cells.Count == 0)
                    {
                        TestContext.WriteLine("⚠️ Row has no <td> cells.");
                        continue;
                    }

                    foreach (var cell in cells)
                    {
                        var rawText = await cell.InnerTextAsync();
                        var normalizedText = Regex.Replace(rawText ?? "", @"\s+", " ").Trim();
                        TestContext.WriteLine($"🔎 Checking cell text: '{normalizedText}'");

                        if (string.Equals(normalizedText, rowText, StringComparison.OrdinalIgnoreCase))
                        {
                            TestContext.WriteLine($"✅ Found exact match in cell: '{normalizedText}'");

                            var actualRow = await cell.EvaluateHandleAsync("node => node.closest('tr')");
                            var icon = await actualRow.AsElement()?.QuerySelectorAsync($"img[src='{iconSRC}']");

                            if (icon != null)
                            {
                                await icon.ClickAsync();
                                TestContext.WriteLine($"🖱️ Clicked icon with src: {iconSRC} in matched row.");
                                return;
                            }
                            else
                            {
                                TestContext.WriteLine($"⚠️ Icon with src: {iconSRC} not found in matched row.");
                                return;
                            }
                        }
                    }
                }

                // If no matching row found, try JS fallback
                TestContext.WriteLine($"❌ No row found with exact text: '{rowText}'. Attempting JS fallback...");

                var executionContext = frame ?? page?.MainFrame;
                if (executionContext == null)
                {
                    TestContext.WriteLine("❌ No valid execution context available for JS fallback.");
                    return;
                }

                string frameInfo;
                try
                {
                    frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                }
                catch
                {
                    frameInfo = executionContext.Name ?? "<no name>";
                }
                TestContext.WriteLine($"🧠 JS fallback executing in: {frameInfo}");

                var escapedRowText = JsonSerializer.Serialize(rowText.ToLowerInvariant());
                var escapedIconSRC = JsonSerializer.Serialize(iconSRC);

                var jsClickIcon = $@"
                    (() => {{
                        const rows = document.querySelectorAll('tr');
                        for (const row of rows) {{
                            const cells = row.querySelectorAll('td');
                            for (const cell of cells) {{
                                const text = cell.textContent.replace(/\s+/g, ' ').trim().toLowerCase();
                                if (text === {escapedRowText}) {{
                                    const icon = row.querySelector(`img[src={escapedIconSRC}]`);
                                    if (icon) {{
                                        ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                            const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                                            icon.dispatchEvent(evt);
                                        }});
                                        return true;
                                    }}
                                }}
                            }}
                        }}
                        return false;
                    }})()";

                var jsClicked = await executionContext.EvaluateAsync<bool>(jsClickIcon);

                if (jsClicked)
                {
                    TestContext.WriteLine($"✅ JS fallback clicked icon with src: {iconSRC} in row: {rowText}");
                }
                else
                {
                    TestContext.WriteLine($"❌ JS fallback failed to click icon with src: {iconSRC} in row: {rowText}");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error clicking icon with src '{iconSRC}' in row '{rowText}': {ex.Message}");
            }
        }


        /// <summary>
        /// Clicks an icon in a given row based on it's img src
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="rowText">The title or text of the row you would like to search for the icon within - this is based on a PARTIAL match</param>
        /// <param name="iconSRC">The img src= text of the icon.</param>
        /// <returns></returns>
        public static async Task ClickIconInRowPartialMatch(string partialRowText, string iconSRC, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Looking for row containing: '{partialRowText}' in {contextName}");

                var rows = page != null
                    ? await page.QuerySelectorAllAsync("tr")
                    : await frame?.QuerySelectorAllAsync("tr");

                if (rows == null || rows.Count == 0)
                {
                    TestContext.WriteLine("❌ No rows found.");
                    return;
                }

                foreach (var row in rows)
                {
                    var rowTextContent = await row.InnerTextAsync();
                    var normalizedRowText = Regex.Replace(rowTextContent ?? "", @"\s+", " ").Trim();

                    TestContext.WriteLine($"🧪 Normalized row text: '{normalizedRowText}'");

                    if (normalizedRowText.Contains(partialRowText, StringComparison.OrdinalIgnoreCase))
                    {
                        TestContext.WriteLine($"✅ Found row containing: '{partialRowText}'");

                        var icon = await row.QuerySelectorAsync($"img[src='{iconSRC}']");
                        if (icon != null)
                        {
                            await icon.ClickAsync();
                            TestContext.WriteLine($"🖱️ Clicked icon with src: {iconSRC} in row containing: '{partialRowText}'");
                            return;
                        }
                        else
                        {
                            TestContext.WriteLine($"⚠️ Icon with src: {iconSRC} not found in matched row.");
                            return;
                        }
                    }
                }

                // JS fallback
                TestContext.WriteLine($"❌ No row matched. Attempting JS fallback for: '{partialRowText}'");

                var executionContext = frame ?? page?.MainFrame;
                if (executionContext == null)
                {
                    TestContext.WriteLine("❌ No valid frame or page context available for JS fallback.");
                    return;
                }

                string frameInfo;
                try
                {
                    frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                }
                catch
                {
                    frameInfo = executionContext.Name ?? "<no name>";
                }

                TestContext.WriteLine($"🧠 JS fallback executing in: {frameInfo}");

                var escapedRowText = JsonSerializer.Serialize(partialRowText.ToLowerInvariant());
                var escapedIconSRC = JsonSerializer.Serialize(iconSRC);

                var jsClickIcon = $@"
                    (() => {{
                        const rows = document.querySelectorAll('tr');
                        for (const row of rows) {{
                            const text = row.textContent.replace(/\s+/g, ' ').trim().toLowerCase();
                            if (text.includes({escapedRowText})) {{
                                const icon = row.querySelector(`img[src={escapedIconSRC}]`);
                                if (icon) {{
                                    ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                        const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                                        icon.dispatchEvent(evt);
                                    }});
                                    return true;
                                }}
                            }}
                        }}
                        return false;
                    }})()";

                var jsClicked = await executionContext.EvaluateAsync<bool>(jsClickIcon);

                if (jsClicked)
                {
                    TestContext.WriteLine($"✅ JS fallback clicked icon with src: {iconSRC} in row containing: '{partialRowText}'");
                }
                else
                {
                    TestContext.WriteLine($"❌ JS fallback failed to click icon with src: {iconSRC} in row containing: '{partialRowText}'");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error clicking icon with src '{iconSRC}' in row containing text '{partialRowText}': {ex.Message}");
            }
        }






        /// <summary>
        /// This function will find an icon and click it based on a DIV and table structure.
        /// </summary>
        /// <param name="divName">The text inside the div</param>
        /// <param name="iconSRC">The icons src= you want to click</param>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="page">The page you would like to search within</param>
        /// <returns></returns>
        public static async Task ClickIconInDivTable(string divName, string iconSRC, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Looking for division: '{divName}' in {contextName}");

                var divs = page != null
                    ? await page.QuerySelectorAllAsync("div.list-group")
                    : await frame?.QuerySelectorAllAsync("div.list-group");

                if (divs == null || divs.Count == 0)
                {
                    TestContext.WriteLine("❌ No division headers found.");
                    return;
                }

                foreach (var div in divs)
                {
                    var divText = await div.InnerTextAsync();
                    var trimmedText = divText?.Trim() ?? "";

                    TestContext.WriteLine($"🧪 Checking division header: '{trimmedText}'");

                    if (trimmedText.Contains(divName, StringComparison.OrdinalIgnoreCase))
                    {
                        TestContext.WriteLine($"✅ Matched division header: '{trimmedText}'");

                        var tableHandle = await div.EvaluateHandleAsync(@"div => {
                            let el = div.nextElementSibling;
                            while (el && el.tagName !== 'TABLE') {
                                el = el.nextElementSibling;
                            }
                            return el;
                        }");

                        var table = tableHandle?.AsElement();
                        if (table == null)
                        {
                            TestContext.WriteLine("⚠️ No table found after the matched division header.");
                            return;
                        }

                        var rows = await table.QuerySelectorAllAsync("tr");
                        if (rows == null || rows.Count == 0)
                        {
                            TestContext.WriteLine("⚠️ No rows found in the division's table.");
                            return;
                        }

                        foreach (var row in rows)
                        {
                            var rowText = await row.InnerTextAsync();
                            TestContext.WriteLine($"🧾 Row text: '{rowText?.Trim()}'");

                            var icon = await row.QuerySelectorAsync($"img[src='{iconSRC}']");
                            if (icon != null)
                            {
                                await icon.ClickAsync();
                                TestContext.WriteLine($"🖱️ Clicked icon with src: {iconSRC} in row.");
                                return;
                            }
                        }

                        TestContext.WriteLine($"❌ Icon with src: {iconSRC} not found in any row of the division's table.");
                        return;
                    }
                }

                // JS Fallback
                TestContext.WriteLine($"❌ No matching div found. Attempting JS fallback for div: '{divName}'");

                var executionContext = frame ?? page?.MainFrame;
                if (executionContext == null)
                {
                    TestContext.WriteLine("❌ No valid frame or page context available for JS fallback.");
                    return;
                }

                string frameInfo;
                try
                {
                    frameInfo = await executionContext.EvaluateAsync<string>("() => window.location.href");
                }
                catch
                {
                    frameInfo = executionContext.Name ?? "<no name>";
                }

                TestContext.WriteLine($"🧠 JS fallback executing in: {frameInfo}");

                var escapedDivName = JsonSerializer.Serialize(divName.ToLowerInvariant());
                var escapedIconSRC = JsonSerializer.Serialize(iconSRC);

                var jsClick = $@"
                    (() => {{
                        const divs = document.querySelectorAll('div.list-group');
                        for (const div of divs) {{
                            const text = div.textContent.trim().toLowerCase();
                            if (text.includes({escapedDivName})) {{
                                let el = div.nextElementSibling;
                                while (el && el.tagName !== 'TABLE') {{
                                    el = el.nextElementSibling;
                                }}
                                if (el) {{
                                    const rows = el.querySelectorAll('tr');
                                    for (const row of rows) {{
                                        const icon = row.querySelector(`img[src={escapedIconSRC}]`);
                                        if (icon) {{
                                            ['mousedown', 'mouseup', 'click'].forEach(type => {{
                                                const evt = new MouseEvent(type, {{ bubbles: true, cancelable: true, view: window }});
                                                icon.dispatchEvent(evt);
                                            }});
                                            return true;
                                        }}
                                    }}
                                }}
                            }}
                        }}
                        return false;
                    }})()";

                var clicked = await executionContext.EvaluateAsync<bool>(jsClick);

                if (clicked)
                {
                    TestContext.WriteLine($"✅ JS fallback clicked icon with src: {iconSRC} in division: '{divName}'");
                }
                else
                {
                    TestContext.WriteLine($"❌ JS fallback failed to click icon with src: {iconSRC} in division: '{divName}'");
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error clicking icon with src '{iconSRC}' in division '{divName}': {ex.Message}");
            }
        }



        /// <summary>
        /// Determines whether an element is visible on screen.
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="locator">The locator of the element you would like to know is visible</param>
        /// <returns></returns>
        public static async Task<bool> IsElementVisible(string locator, IFrame? frame = null, IPage? page = null)
        {
            var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";

            try
            {
                TestContext.WriteLine($"🔍 Checking visibility of element: {locator} in {contextName}");

                var result = await GetElement(locator, frame, page);

                if (result.Handle != null)
                {
                    var isVisible = await result.Handle.IsVisibleAsync();
                    TestContext.WriteLine($"✅ Element {locator} visibility via Playwright: {isVisible}");
                    return isVisible;
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    TestContext.WriteLine($"⚠️ Playwright failed. Checking visibility via JS fallback: {result.JsSelector}");

                    var jsCheck = $@"
                        (() => {{
                            const el = {result.JsSelector};
                            if (!el) return false;
                            const style = window.getComputedStyle(el);
                            return style && style.display !== 'none' && style.visibility !== 'hidden' && el.offsetHeight > 0 && el.offsetWidth > 0;
                        }})()
                    ";

                    var isVisible = await (page ?? frame?.Page).EvaluateAsync<bool>(jsCheck);
                    TestContext.WriteLine($"✅ Element {locator} visibility via JS fallback: {isVisible}");
                    return isVisible;
                }
                else
                {
                    TestContext.WriteLine($"❌ Element {locator} not found in {contextName} via Playwright or JS fallback.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error checking visibility of element {locator} in {contextName}: {ex.Message}");
                return false;
            }
        }




        /// <summary>
        /// Waits for an element to be visible on screen with a specified timeout.
        /// </summary>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="selector">The locator of the element you would like to know is visible</param>
        /// <param name="timeout">How long the timeout should be. Default is 30000 (30 seconds)</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task<bool> WaitForElementToBeVisible(string selector, IFrame? frame = null, IPage? page = null, int timeout = 30000)
        {
            if (frame == null && page == null)
                throw new InvalidOperationException("Cannot wait for selector, both frame and page are null.");

            var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
            TestContext.WriteLine($"⏳ Waiting for element to become visible: {selector} in {contextName}");

            try
            {
                var result = await GetElement(selector, frame, page);

                if (result.Handle != null)
                {
                    TestContext.WriteLine($"✅ Element: {selector} found via Playwright. Waiting for visibility...");

                    await result.Handle.WaitForElementStateAsync(ElementState.Visible, new() { Timeout = timeout });
                    TestContext.WriteLine($"✅ Element: {selector} is now visible in {contextName}.");
                    return true;
                }
                else if (!string.IsNullOrEmpty(result.JsSelector))
                {
                    TestContext.WriteLine($"⚠️ Playwright failed. Using JS fallback to wait for visibility: {result.JsSelector}");

                    var jsWait = $@"
                        async function waitForVisibility(selector, timeout) {{
                            const start = Date.now();
                            while (Date.now() - start < timeout) {{
                                const el = {result.JsSelector};
                                if (el) {{
                                    const style = window.getComputedStyle(el);
                                    if (style.display !== 'none' && style.visibility !== 'hidden' && el.offsetHeight > 0 && el.offsetWidth > 0) {{
                                        return true;
                                    }}
                                }}
                                await new Promise(r => setTimeout(r, 250));
                            }}
                            return false;
                        }}
                        return await waitForVisibility('{selector}', {timeout});
                    ";

                    var isVisible = await (page ?? frame?.Page).EvaluateAsync<bool>(jsWait);
                    TestContext.WriteLine($"✅ JS fallback visibility result for {selector}: {isVisible}");
                    return isVisible;
                }
                else
                {
                    TestContext.WriteLine($"❌ Element: {selector} not found in {contextName} via Playwright or JS fallback.");
                    return false;
                }
            }
            catch (TimeoutException)
            {
                TestContext.WriteLine($"⌛ Timeout waiting for element: {selector} to become visible in {contextName}.");
                return false;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error waiting for visibility of element {selector} in {contextName}: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Finds and returns an element in a table using a string for the element you want to find.
        /// </summary>
        /// <param name="searchString">The text you would like to search for in the table</param>
        /// <param name="frame">The iFrame you would like to search within</param>
        /// <param name="page">The page you would like to search within</param>
        /// <returns></returns>
        public static async Task<IElementHandle?> FindStringInTable(string searchString, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Searching for string '{searchString}' in tables on {contextName}");

                var tables = page != null
                    ? await page.QuerySelectorAllAsync("table.LinedList")
                    : await frame?.QuerySelectorAllAsync("table.LinedList");

                TestContext.WriteLine($"Found {tables?.Count ?? 0} tables with class 'LinedList'.");

                if (tables == null || tables.Count == 0)
                {
                    TestContext.WriteLine("No tables found to search.");
                    return null;
                }

                foreach (var table in tables)
                {
                    TestContext.WriteLine("Checking table...");

                    var elements = await table.QuerySelectorAllAsync("b, tr, td, div, span");
                    TestContext.WriteLine($"Found {elements.Count} elements in the current table.");

                    foreach (var element in elements)
                    {
                        var rawText = await element.TextContentAsync();
                        var normalizedText = Regex.Replace(rawText ?? "", @"\s+", " ").Trim();

                        TestContext.WriteLine($"Checking normalized text: '{normalizedText}'");

                        if (!string.IsNullOrEmpty(normalizedText) &&
                            normalizedText.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        {
                            TestContext.WriteLine($"✅ Found matching text: '{normalizedText}'");
                            return element;
                        }
                    }
                }

                TestContext.WriteLine($"❌ String '{searchString}' not found via Playwright. Attempting JS fallback...");

                var jsSearch = $@"
                    const tables = document.querySelectorAll('table.LinedList');
                    for (const table of tables) {{
                        const elements = table.querySelectorAll('b, tr, td, div, span');
                        for (const el of elements) {{
                            const text = el.textContent.replace(/\s+/g, ' ').trim().toLowerCase();
                            if (text.includes('{searchString.ToLower()}')) {{
                                el.scrollIntoView({{ behavior: 'smooth', block: 'center' }});
                                return true;
                            }}
                        }}
                    }}
                    return false;
                ";

                var foundViaJs = await (page ?? frame?.Page).EvaluateAsync<bool>(jsSearch);
                TestContext.WriteLine(foundViaJs
                    ? $"✅ JS fallback found and scrolled to string '{searchString}'"
                    : $"❌ JS fallback did not find string '{searchString}'");

                // JS fallback can only scroll, no element handle returned
                return null;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error searching for string '{searchString}' in table: {ex.Message}");
                return null;
            }
        }



        public static async Task<IElementHandle?> FindPartialStringInTable(string searchText, IFrame? frame = null, IPage? page = null)
        {
            try
            {
                var contextName = page != null ? "page" : $"frame: {frame?.Name ?? "unknown"}";
                TestContext.WriteLine($"🔍 Searching for partial match: '{searchText}' in tables on {contextName}");

                var tables = page != null
                    ? await page.QuerySelectorAllAsync("table.LinedList")
                    : await frame?.QuerySelectorAllAsync("table.LinedList");

                if (tables == null || tables.Count == 0)
                {
                    TestContext.WriteLine("No tables found to search.");
                    return null;
                }

                foreach (var table in tables)
                {
                    var rows = await table.QuerySelectorAllAsync("tr");
                    foreach (var row in rows)
                    {
                        var cells = await row.QuerySelectorAllAsync("td");
                        foreach (var cell in cells)
                        {
                            var rawText = await cell.InnerTextAsync();
                            var text = Regex.Replace(rawText ?? "", @"\s+", " ").Trim();

                            TestContext.WriteLine($"Checking cell text: '{text}'");

                            if (!string.IsNullOrEmpty(text) &&
                                text.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                            {
                                TestContext.WriteLine($"✅ Found partial match: '{text}'");
                                return cell;
                            }
                        }
                    }
                }

                TestContext.WriteLine($"❌ Partial match '{searchText}' not found via Playwright. Attempting JS fallback...");

                var jsSearch = $@"
                    const tables = document.querySelectorAll('table.LinedList');
                    for (const table of tables) {{
                        const cells = table.querySelectorAll('td');
                        for (const cell of cells) {{
                            const text = cell.textContent.replace(/\s+/g, ' ').trim().toLowerCase();
                            if (text.includes('{searchText.ToLower()}')) {{
                                cell.scrollIntoView({{ behavior: 'smooth', block: 'center' }});
                                return true;
                            }}
                        }}
                    }}
                    return false;
                ";

                var foundViaJs = await (page ?? frame?.Page).EvaluateAsync<bool>(jsSearch);
                TestContext.WriteLine(foundViaJs
                    ? $"✅ JS fallback found and scrolled to partial match '{searchText}'"
                    : $"❌ JS fallback did not find partial match '{searchText}'");

                // JS fallback can't return IElementHandle
                return null;
            }
            catch (Exception ex)
            {
                TestContext.WriteLine($"💥 Error searching for partial match '{searchText}' in table: {ex.Message}");
                return null;
            }
        }


    }
}
