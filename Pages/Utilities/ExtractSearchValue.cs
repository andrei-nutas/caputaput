using System.Text.RegularExpressions;

namespace TAF_iSAMS.Pages.Utilities
{
    public static class ExtractSearchValue
    {
        public static string ExtractAndReturnSearchValue(string locator)
        {
            // Handle has-text('...') or has-text("...")
            var hasTextMatch = Regex.Match(locator, @"has-text\((?:'([^']*)'|""([^""]*)"")\)");
            if (hasTextMatch.Success)
                return hasTextMatch.Groups[1].Success ? hasTextMatch.Groups[1].Value : hasTextMatch.Groups[2].Value;

            // Match title attribute: title='...' or title="..."
            var titleMatch = Regex.Match(locator, @"title=(?:'([^']*)'|""([^""]*)"")");
            if (titleMatch.Success)
                return titleMatch.Groups[1].Success ? titleMatch.Groups[1].Value : titleMatch.Groups[2].Value;

            // Match ID selector e.g. #selectall => selectall
            var idMatch = Regex.Match(locator, @"^#([\w\-]+)$");
            if (idMatch.Success)
                return idMatch.Groups[1].Value;

            // Match name attribute e.g. [name='txtName'] or name='txtName'
            var nameMatch = Regex.Match(locator, @"name\s*=\s*['""]?([^'""]+)['""]?");
            if (nameMatch.Success)
                return nameMatch.Groups[1].Value;

            // If it's just a plain string (like "Year 14 (14)"), return it
            if (!locator.Contains(":") && !locator.Contains("[") && !locator.Contains("="))
                return locator;

            // Fallback to returning as-is (e.g. valid JS selector like div.button)
            return locator;
        }

    }

}
