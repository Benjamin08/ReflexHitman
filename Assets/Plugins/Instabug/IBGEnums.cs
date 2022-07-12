namespace Plugins.Instabug
{
    public enum IBGDismissType
    {
        Submit = 0,
        Cancel = 1,
        AddAttachment = 2
    }

    public enum IBGReportType
    {
        Bug = 0,
        Feedback = 1
    }

    public enum IBGBugReportingReportType
    {
        Bug = 1 << 0,
        Feedback = 1 << 1,
        Question = 1 << 2
    }

    public enum IBGBugReportingInvocationOption 
    {
        EmailFieldHidden = 1 << 0,
        EmailFieldOptional = 1 << 1,
        CommentFieldRequired = 1 << 2,
        DisablePostSendingDialog = 1 << 3
    }

    public enum IBGWelcomeMessageMode
    {
        Live = 0,
        Beta = 1,
        Disabled = 2
    }

    public enum IBGInvocationEvent
    {
        Shake = 1 << 0,
        Screenshot = 1 << 1,
        TwoFingerSwipeLeft = 1 << 2,
        RightEdgePan = 1 << 3,
        FloatingButton = 1 << 4,
        None = 1 << 5
    }
}
