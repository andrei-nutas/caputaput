namespace TAF_iSAMS.Pages.Utilities
{
    public static class FrameRetryConfig
    {
        public static int MaxRetries { get; set; } = 2;
        public static int RetryDelayMs { get; set; } = 1000;
        public static bool EnableLogging { get; set; } = true;

        public static bool IsFrameRelatedError(Exception ex)
        {
            var message = ex.Message.ToLowerInvariant();
            return message.Contains("timeout") ||
                   message.Contains("element not found") ||
                   message.Contains("frame") ||
                   message.Contains("detached") ||
                   message.Contains("cannot find") ||
                   message.Contains("navigator is not initialized") ||
                   ex is TimeoutException ||
                   ex is InvalidOperationException;
        }
    }
}