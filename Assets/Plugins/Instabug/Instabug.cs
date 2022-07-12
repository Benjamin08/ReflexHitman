using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Plugins.Instabug
{
    public class Instabug : MonoBehaviour
    {
        private static Instabug instance;

        public delegate void PreSendingCallbackDelegate();
        public delegate void PreInvocationCallbackDelegate();
        public delegate void PostInvocationCallbackDelegate(IBGDismissType dismissType, IBGReportType reportType);

        [Header("Instabug Setup")]
        public string appToken = "your-instabug-app-token";
        public IBGInvocationEvent invocationEvent;

#if (UNITY_IOS && !UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern void Instabug_InitializeInstabug(string appToken, int invocationEvent);
        [DllImport("__Internal")]
        private static extern void Instabug_Show();
        [DllImport("__Internal")]
        private static extern void Instabug_showWithReportTypeAndOptions(int reportType, int options);
        [DllImport("__Internal")]
        private static extern void Instabug_setBugReportingOptions(int options);
        [DllImport("__Internal")]
        private static extern void Instabug_Dismiss();
        [DllImport("__Internal")]
        private static extern void Instabug_AddFileAttachmentWithURL(string url);
        [DllImport("__Internal")]
        private static extern void Instabug_ClearFileAttachments();
        [DllImport("__Internal")]
        private static extern void Instabug_SetUserData(string userData);
        [DllImport("__Internal")]
        private static extern void Instabug_SetPreSendingHandler(PreSendingCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void Instabug_SetPreInvocationHandler(PreInvocationCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void Instabug_SetPostInvocationHandler(PostInvocationCallbackDelegate callback);
        [DllImport("__Internal")]
        private static extern void Instabug_IdentifyUserWithEmail(string email, string name);
        [DllImport("__Internal")]
        private static extern void Instabug_LogOut();
        [DllImport("__Internal")]
        private static extern void Instabug_SetAttachmentTypesEnabled(bool screenshot, bool extraScreenshot, bool galleryImage, bool screenRecording);
        [DllImport("__Internal")]
        private static extern void Instabug_ShowWelcomeMessageWithMode(int welcomeMessageMode);
        [DllImport("__Internal")]
        private static extern void Instabug_LogDebug(string log);
        [DllImport("__Internal")]
        private static extern void Instabug_LogVerbose(string log);
        [DllImport("__Internal")]
        private static extern void Instabug_LogWarn(string log);
        [DllImport("__Internal")]
        private static extern void Instabug_LogInfo(string log);
        [DllImport("__Internal")]
        private static extern void Instabug_LogError(string log);
        [DllImport("__Internal")]
        private static extern void Instabug_SetAutoScreenRecordingEnabled(bool isEnabled);
        [DllImport("__Internal")]
        private static extern void Instabug_SetAutoScreenRecordingAudioCapturingEnabled(bool isEnabled);
        [DllImport("__Internal")]
        private static extern void Instabug_SetAutoScreenRecordingDuration(int duration);
        [DllImport("__Internal")]
        private static extern void Instabug_recordVisualUserStepForView(string viewName);
        [DllImport("__Internal")]
        private static extern void Instabug_recordUserStep(string stepName, string objectName);
        [DllImport("__Internal")]
        private static extern void Instabug_SetWelcomeMessageMode(int mode);
        [DllImport("__Internal")]
        private static extern void Instabug_AddFileAttachmentWithData(string data);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
        private static AndroidJavaObject jInstabug;
        private static AndroidJavaObject currentActivity;
#endif

        private void Awake()
        {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
#endif
        }

        private void OnEnable()
        {
            PreInit();
            InitializeInstabug();

            instance = this;
            CaptureTouchEventsOnGameObjects();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        void OnDisable()
        {
                SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        void OnSceneChanged(Scene current, Scene next) 
        {
                StartCoroutine(ExecuteAfterTime(0.5f, next));
        }

        IEnumerator ExecuteAfterTime(float time, Scene scene)
        {
        yield return new WaitForSeconds(time);
        Debug.Log("OnSceneLoaded: " + scene.name);
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_recordVisualUserStepForView(scene.name);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            CaptureScreenshot(scene.name);
#endif
        }

        void Update()
        {
            CaptureTouchEventsOnGameObjects();

        }

        private void InitializeInstabug()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_InitializeInstabug(appToken, (int)invocationEvent);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("invokeInstabug", currentActivity, appToken, (int)invocationEvent);
#endif
        }

        private static void PreInit()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            using (var unityPlayer = new AndroidJavaClass(IBGConstants.JAVA_UNITYPLAYER_CLASS))
            {
                jInstabug = new AndroidJavaClass(IBGConstants.JAVA_INSTABUGUNITYPLUGIN_CLASS);
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
#endif
        }

        /// <summary>
        /// Invokes the SDK manually with the default invocation mode.
        /// </summary>
        public static void Invoke()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_Show();
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("show");
#endif
        }

        /// <summary>
        /// shows the SDK with a specific mode.
        /// </summary>
        /// <param name="reportType">Specifies which mode the SDK is going to start with.</param>
        public static void ShowWithReportTypeAndOptions(IBGBugReportingReportType reportType, IBGBugReportingInvocationOption[] options)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            int optionsEnum = 0;
            foreach (IBGBugReportingInvocationOption o in options)
            {
               optionsEnum |= (int)o;
            }
            Instabug_showWithReportTypeAndOptions((int)reportType, optionsEnum);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            int[] optionsEnums = new int[options.Length];
            for (int i = 0; i< options.Length; i++) {
                    optionsEnums[i] = (int) options[i];
            }
            jInstabug.CallStatic("showWithReportTypeAndOptions", (int)reportType, optionsEnums);
#endif
        }
        
        /// <summary>
        /// manipulate email and comment fields requirement
        /// </summary>
        /// <param name="options">Specifies BugReport options.</param>
        public static void SetBugReportOptions(IBGBugReportingInvocationOption[] options)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            int optionsEnum = 0;
            foreach (IBGBugReportingInvocationOption o in options)
            {
               optionsEnum |= (int)o;
            }
            Instabug_setBugReportingOptions(optionsEnum);
#elif (UNITY_ANDROID && !UNITY_EDITOR)

            int[] optionsEnums = new int[options.Length];
            for (int i = 0; i < options.Length; i++)
            {
                optionsEnums[i] = (int)options[i];
            }
            jInstabug.CallStatic("setBugReportingOptions", optionsEnums);
#endif
        }

        [Obsolete]
        /// <summary>
        /// Dismisses any Instabug views that are currently being shown.
        /// </summary>
        public static void Dismiss()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_Dismiss();
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("dismiss");
#endif
        }

        /// <summary>
        /// Add file to attached files with each report being sent.
        /// </summary>
        /// <param name="url">Path to a file that's going to be attached to each report.</param>
        public static void AddFileAttachmentWithURL(string url)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_AddFileAttachmentWithURL(url);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("addFileAttachmentWithUrl",  url);
#endif
        }

        public static void AddFileAttachmentWithData(string data, string fileName)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_AddFileAttachmentWithData(data);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("addFileAttachmentWithData",  data, fileName);
#endif
        }

        /// <summary>
        /// Clear list of files to be attached with each report.
        /// </summary>
        public static void ClearFileAttachments()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_ClearFileAttachments();
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("clearFileAttachments");
#endif
        }

        /// <summary>
        /// Attaches user data to each report being sent.
        /// </summary>
        /// <param name="userData">A string to be attached to each report, with a maximum size of 1,000 characters.</param>
        public static void SetUserData(string userData)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetUserData(userData);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setUserData",  userData);
#endif
        }

        /// <summary>
        /// Sets a block of code to be executed before sending each report.
        /// </summary>
        /// <param name="preSendingCallback">A block of code that gets executed before sending each bug report.</param>
        public static void SetPreSendingHandler(PreSendingCallbackDelegate preSendingCallback)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetPreSendingHandler(preSendingCallback);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setPreSendingHandler",  new AndroidJavaRunnable(preSendingCallback));
#endif
        }

        /// <summary>
        /// Sets a block of code to be executed just before the SDK's UI is presented
        /// </summary>
        /// <param name="preInvocationCallback">A block of code that gets executed before presenting the SDK's UI.</param>
        public static void SetPreInvocationHandler(PreInvocationCallbackDelegate preInvocationCallback)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetPreInvocationHandler(preInvocationCallback);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setPreInvocationHandler",   new AndroidJavaRunnable(preInvocationCallback));
#endif
        }

        public static void SetPostInvocationHandler(PostInvocationCallbackDelegate postInvocationCallback) {

#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetPostInvocationHandler(postInvocationCallback);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
             IBGAndroidCallbacks.AndroidOnSDKDismissedCallback callback = new IBGAndroidCallbacks.AndroidOnSDKDismissedCallback(postInvocationCallback);
            jInstabug.CallStatic("setPostInvocationHandler",   callback);
#endif
        }

        /// <summary>
        /// Sets the user email and name for all sent reports.
        /// </summary>
        /// <param name="email">Email address to be set as the user's email.</param>
        /// <param name="name">Name of the user to be set.</param>
        public static void IdentifyUserWithEmail(string email, string name)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_IdentifyUserWithEmail(email, name);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("identifyUserWithEmail",   email, name);
#endif
        }

        /// <summary>
        /// Resets the value of the user's email and name, previously set using IdentifyUserWithEmail
        /// </summary>
        public static void LogOut()
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogOut();
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logOut", currentActivity);
#endif
        }

        /// <summary>
        /// Present a view that educates the user on how to invoke the SDK with the currently set invocation event.
        /// </summary>
        public static void ShowWelcomeMessageWithMode(IBGWelcomeMessageMode welcomeMessageMode)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_ShowWelcomeMessageWithMode((int)welcomeMessageMode);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("showWelcomeMessageWithMode",   (int)welcomeMessageMode);
#endif
        }

                /// <summary>
        /// Alows setting bug report attachements
        /// </summary>
        public static void SetAttachmentTypesEnabled(Boolean screenshot, Boolean extraScreenshot, Boolean galleryImage, Boolean screenRecording)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetAttachmentTypesEnabled(screenshot, extraScreenshot, galleryImage, screenRecording );
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setAttachmentTypesEnabled", screenshot, extraScreenshot, galleryImage, screenRecording);
#endif
        }

        /// <summary>
        /// Adds a debug log to be attached to the report.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void LogDebug(string log)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogDebug(log);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logDebug",   log);
#endif
        }

        /// <summary>
        /// Adds a verbose log to be attached to the report.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void LogVerbose(string log)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogVerbose(log);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logVerbose",   log);
#endif
        }

        /// <summary>
        /// Adds a info log to be attached to the report.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void LogInfo(string log)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogInfo(log);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logInfo",   log);
#endif
        }

        /// <summary>
        /// Adds a warn log to be attached to the report.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void LogWarn(string log)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogWarn(log);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logWarn",   log);
#endif
        }

        /// <summary>
        /// Adds a error log to be attached to the report.
        /// </summary>
        /// <param name="log">The log to add.</param>
        public static void LogError(string log)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_LogError(log);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("logError",   log);
#endif
        }

        /// <summary>
        /// Enables or disables auto screen recording to be attached with sent reports.
        /// </summary>
        /// <param name="isEnabled">Boolean to enable or disable the feature.</param>
        public static void SetAutoScreenRecordingEnabled(Boolean isEnabled)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetAutoScreenRecordingEnabled(isEnabled);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setAutoScreenRecordingEnabled",   isEnabled);
#endif
        }

        /// <summary>
        /// Sets whether the Instabug welcome message should appear in live mode, beta mode or disables it.
        /// </summary>
        /// <param name="mode">The mode for the welcome message.</param>
        public static void SetWelcomeMessageMode(IBGWelcomeMessageMode welcomeMessageMode)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetWelcomeMessageMode((int) welcomeMessageMode);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setWelcomeMessageMode",   (int)welcomeMessageMode);
#endif
        }

        private static void SetAutoScreenRecordingAudioCapturingEnabled(Boolean isEnabled)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetAutoScreenRecordingAudioCapturingEnabled(isEnabled);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setAutoScreenRecordingAudioCapturingEnabled",   isEnabled);
#endif
        }

        private static void SetAutoScreenRecordingDuration(int duration)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            Instabug_SetAutoScreenRecordingDuration(duration);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("setAutoScreenRecordingDuration",   duration);
#endif
        }

        private void CaptureTouchEventsOnGameObjects()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;

                if (Physics.Raycast(raycast, out raycastHit))
                {
                    Debug.Log("3D gameObject was hit: " + raycastHit.collider.ToString());
#if (UNITY_IOS && !UNITY_EDITOR)
                    Instabug_recordUserStep ("tap", raycastHit.collider.ToString ());
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                    jInstabug.CallStatic("addStepWithinScene", "Tap", raycastHit.collider.ToString());
#endif
                    return;
                }

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
#if (UNITY_IOS && !UNITY_EDITOR)
                    Instabug_recordUserStep ("tap", EventSystem.current.currentSelectedGameObject.name);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                    jInstabug.CallStatic("addStepWithinScene", "Tap", EventSystem.current.currentSelectedGameObject.name);
#endif

                    return;
                }
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);

                RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.GetTouch(0).position));
                if (rayHit)
                {
                    Debug.Log("2D gameObject was hit: " + rayHit.collider.ToString());
#if (UNITY_IOS && !UNITY_EDITOR)
                    Instabug_recordUserStep ("tap", rayHit.collider.ToString ());
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                    jInstabug.CallStatic("addStepWithinScene", "Tap",rayHit.collider.ToString());
#endif
                }
            }
        }

        public static void CaptureScreenshot(string sceneName)
        {
            string filePath = Application.persistentDataPath + "/ibgImage";
            ScreenCapture.CaptureScreenshot("ibgImage");
#if (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug.CallStatic("sendScreenshotToNativeSDK", filePath, sceneName);
#endif
        }


    }


}
