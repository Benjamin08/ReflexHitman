using System;
namespace Plugins.Instabug
{
    internal static class IBGHelpers
    {
        internal static IBGDismissType GetDismissType(string dismissType)
        {
            IBGDismissType eDismissType;
            switch (dismissType)
            {
                case "submit": eDismissType = IBGDismissType.Submit; break;
                case "add_attachment": eDismissType = IBGDismissType.AddAttachment; break;
                default: eDismissType = IBGDismissType.Cancel; break;
            }
            return eDismissType;
        }

        internal static IBGReportType GetReportType(string reportType)
        {
            if (reportType.Equals("feedback"))
            {
                return IBGReportType.Feedback;
            }
            else
            {
                return IBGReportType.Bug;
            }
        }
    }
}
