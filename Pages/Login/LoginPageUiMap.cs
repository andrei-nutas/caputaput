namespace TAF_iSAMS.Pages.Login
{
    /// <summary>
    /// Contains selectors (locators) for the iSAMS Login Page.
    /// No methods, just constants for references in the page logic class.
    /// </summary>
    public static class LoginPageUiMap
    {
        public const string UsernameSelector = "#login-username";
        public const string PasswordSelector = "#login-password";
        public const string LoginButtonSelector = "#login-button";

        public const string SimultaneousLoginButtonSelector =
            "button:has-text('Override')";
    }
}