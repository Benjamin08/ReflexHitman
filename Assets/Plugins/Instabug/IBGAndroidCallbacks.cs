using System;
using UnityEngine;
using Plugins.Instabug;
/*
 * Class for adding any native android callbacks with parameters. It should not be placed in a namespace.
 */
public class IBGAndroidCallbacks
{
    public class AndroidOnSDKDismissedCallback : AndroidJavaProxy
    {
        private Instabug.PostInvocationCallbackDelegate postInvocationCallback;
        public AndroidOnSDKDismissedCallback(Instabug.PostInvocationCallbackDelegate postInvocationCallback) : base("com.example.unityandroid.OnUnitySdkDismissed") 
        {
            this.postInvocationCallback = postInvocationCallback;
        }

        public void onSdkDismissed(String dismissType, String reportType)
        {
            postInvocationCallback(IBGHelpers.GetDismissType(dismissType), IBGHelpers.GetReportType(reportType));
        }

    }
}

