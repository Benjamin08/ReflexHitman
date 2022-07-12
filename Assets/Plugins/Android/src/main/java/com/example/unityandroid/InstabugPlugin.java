package com.example.unityandroid;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.util.Log;

import com.example.unityandroid.utils.MainThreadHandler;
import com.instabug.bug.BugReporting;
import com.instabug.bug.invocation.Option;
import com.instabug.library.Feature;
import com.instabug.library.Instabug;
import com.instabug.library.OnSdkDismissCallback;
import com.instabug.library.invocation.InstabugInvocationEvent;
import com.instabug.library.invocation.OnInvokeCallback;
import com.instabug.library.logging.InstabugLog;
import com.instabug.library.model.Report;
import com.instabug.library.ui.onboarding.WelcomeMessage;
import com.instabug.library.visualusersteps.State;
import com.instabug.library.Platform;

import java.io.File;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;


/**
 * Created by seltarzi on 3/11/18.
 */

public class InstabugPlugin {

    public static void invokeInstabug(final Activity currentActivity, final String token, final int invocationEvent) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugInvocationEvent invocationEventEnum = InstabugInvocationEvent.SHAKE;
                switch (invocationEvent) {
                    case 1:
                        invocationEventEnum = InstabugInvocationEvent.SHAKE;
                        break;
                    case 2:
                        invocationEventEnum = InstabugInvocationEvent.SCREENSHOT;
                        break;
                    case 4:
                        invocationEventEnum = InstabugInvocationEvent.TWO_FINGER_SWIPE_LEFT;
                        break;
                    case 16:
                        invocationEventEnum = InstabugInvocationEvent.FLOATING_BUTTON;
                        break;
                    case 32:
                        invocationEventEnum = InstabugInvocationEvent.NONE;
                        break;
                }
                 try {
                    Class<?> clazz = Class.forName("com.instabug.library.Instabug");
                    Method method = clazz.getDeclaredMethod("setCurrentPlatform",int.class);
                    method.setAccessible(true);
                    method.invoke(null, Platform.UNITY);
                } catch (ClassNotFoundException e) {
                    e.printStackTrace();
                } catch (NoSuchMethodException e) {
                    e.printStackTrace();
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                } catch (InvocationTargetException e) {
                    e.printStackTrace();
                }
                new Instabug.Builder(currentActivity.getApplication(), token)
                        .setInvocationEvents(invocationEventEnum)
                        .build();
                enableScreenShotByMediaProjection();
                enableCPReproSteps();
            }
        });
    }

    /**
     * Enables taking screenshots by media projection.
     */
    private static void enableScreenShotByMediaProjection() {
        try {
            Method method = getMethod(Class.forName("com.instabug.bug.BugReporting"), "setScreenshotByMediaProjectionEnabled", boolean.class);
            if (method != null) {
                method.invoke(null, true);
            }
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        } catch (InvocationTargetException e) {
            e.printStackTrace();
        }
    }


    public static void logDebug(final String log) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugLog.d(log);
            }
        });
    }

    public static void logVerbose(final String log) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugLog.v(log);
            }
        });
    }

    public static void logError(final String log) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugLog.e(log);
            }
        });
    }

    public static void logWarn(final String log) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugLog.w(log);
            }
        });
    }

    public static void logInfo(final String log) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                InstabugLog.i(log);
            }
        });
    }

    public static void setAutoScreenRecordingEnabled(final boolean isEnabled) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                BugReporting.setAutoScreenRecordingEnabled(isEnabled);
            }
        });
    }

    public static void setAutoScreenRecordingDuration(final int duration) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                try {
                    Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "setAutoScreenRecordingDuration", int.class);
                    if (method != null) {
                        try {
                            method.invoke(null, duration);
                        } catch (IllegalAccessException e) {
                            e.printStackTrace();
                        } catch (InvocationTargetException e) {
                            e.printStackTrace();
                        }
                    }
                } catch (ClassNotFoundException e) {
                    e.printStackTrace();
                }
            }
        });

    }


    public static void enableCPReproSteps() {
        Instabug.setReproStepsState(State.DISABLED);
        try {
            Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "setRnReproStepsState", State.class);
            if (method != null) {
                method.invoke(null, State.ENABLED);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static void addVisualUserStepForScene(final String sceneName) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                try {
                    Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "reportScreenChange", Bitmap.class, String.class);
                    if (method != null) {
                        method.invoke(null, null, sceneName);
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public static void addStepWithinScene(final String stepName, final String objectName) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {

                try {
                    Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "addEvent", String.class, String.class);
                    if (method != null) {
                        Log.d("USERSTEPS", "found method addStepWithinScene");
                        method.invoke(null, stepName, objectName);
                    }
                } catch (ClassNotFoundException e) {
                    e.printStackTrace();
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                } catch (InvocationTargetException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    public static void setAutoScreenRecordingAudioCapturingEnabled(final boolean isEnabled) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                try {
                    Log.d("screenRecordingDuration", "Enabling auto screen audio");
                    Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "setAutoScreenRecordingAudioCapturingEnabled", Feature.State.class);
                    if (method != null) {
                        try {
                            Feature.State state = isEnabled ? Feature.State.ENABLED : Feature.State.DISABLED;
                            method.invoke(null, state);
                        } catch (IllegalAccessException e) {
                            e.printStackTrace();
                        } catch (InvocationTargetException e) {
                            e.printStackTrace();
                        }
                    }
                } catch (ClassNotFoundException e) {
                    e.printStackTrace();
                }
            }
        });
    }

    /**
     * Gets the private method that matches the class, method name and parameter types given and making it accessible.
     * For private use only.
     *
     * @param clazz         the class the method is in
     * @param methodName    the method name
     * @param parameterType list of the parameter types of the method
     * @return the method that matches the class, method name and param types given
     */
    public static Method getMethod(Class clazz, String methodName, Class... parameterType) {
        final Method[] methods = clazz.getDeclaredMethods();
        for (Method method : methods) {
            if (method.getName().equals(methodName) && method.getParameterTypes().length ==
                    parameterType.length) {
                for (int i = 0; i < parameterType.length; i++) {
                    if (method.getParameterTypes()[i] == parameterType[i]) {
                        if (i == method.getParameterTypes().length - 1) {
                            method.setAccessible(true);
                            return method;
                        }
                    } else {
                        break;
                    }
                }
            }
        }
        return null;
    }

    public static void show() {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.show();
            }
        });
    }

    @SuppressLint("WrongConstant")
    public static void showWithReportTypeAndOptions( final int reportType, final int[] options) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                int[] optionsEnums = new int[options.length];
                for (int i = 0; i < options.length; i++) {
                    switch (options[i]) {
                        case 1:
                            optionsEnums[i] = Option.EMAIL_FIELD_HIDDEN;
                            break;
                        case 2:
                            optionsEnums[i] = Option.EMAIL_FIELD_OPTIONAL;
                            break;
                        case 4:
                            optionsEnums[i] = Option.COMMENT_FIELD_REQUIRED;
                            break;
                        case 8:
                            optionsEnums[i] = Option.DISABLE_POST_SENDING_DIALOG;
                            break;
                    }
                }
                int reportTypeEnum = BugReporting.ReportType.BUG;
                switch (reportType) {
                    case 1:
                        reportTypeEnum = BugReporting.ReportType.BUG;
                        break;
                    case 2:
                        reportTypeEnum = BugReporting.ReportType.FEEDBACK;
                        break;
                    case 4:
                        reportTypeEnum = BugReporting.ReportType.QUESTION;
                        break;
                }
                BugReporting.show(reportTypeEnum, optionsEnums);
            }
        });
    }
    @SuppressLint("WrongConstant")
        public static void setBugReportingOptions(final int[] options) {
            MainThreadHandler.runOnMainThread(new Runnable() {
                @Override
                public void run() {
                    int[] optionsEnums = new int[options.length];
                    for (int i = 0; i < options.length; i++) {
                        switch (options[i]) {
                            case 1:
                                optionsEnums[i] = Option.EMAIL_FIELD_HIDDEN;
                                break;
                            case 2:
                                optionsEnums[i] = Option.EMAIL_FIELD_OPTIONAL;
                                break;
                            case 4:
                                optionsEnums[i] = Option.COMMENT_FIELD_REQUIRED;
                                break;
                            case 8:
                                optionsEnums[i] = Option.DISABLE_POST_SENDING_DIALOG;
                                break;
                        }
                        BugReporting.setOptions(optionsEnums[i]);
                    }

                }
            });
        }
    @Deprecated
    public static void dismiss() {

//        currentActivity.runOnUiThread(new Runnable() {
//            @Override
//            public void run() {
//                Instabug.dismiss();
//            }
//        });
    }

    public static void addFileAttachmentWithUrl(String url) {
        final File file = new File(url);
        String[] fileArr = url.split("/");
        final String fileNameWithExtension = fileArr[fileArr.length - 1];
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.addFileAttachment(Uri.fromFile(file), fileNameWithExtension);
            }
        });
    }

    public static void addFileAttachmentWithData(final byte[] data, final String fileName) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.addFileAttachment(data, fileName);
            }
        });
    }

    public static void clearFileAttachments() {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.clearFileAttachment();
            }
        });
    }

    public static void setUserData(final String userData) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.setUserData(userData);
            }
        });
    }

    public static void setPreSendingHandler(final Runnable preSendingRunnable) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.onReportSubmitHandler(new Report.OnReportCreatedListener() {
                    @Override
                    public void onReportCreated(Report report) {
                        preSendingRunnable.run();
                    }
                });
            }
        });
    }

    public static void setPreInvocationHandler(final Runnable preInvocationRunnable) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                BugReporting.setOnInvokeCallback(new OnInvokeCallback() {
                    @Override
                    public void onInvoke() {
                        preInvocationRunnable.run();
                    }
                });
            }
        });
    }

    public static void setPostInvocationHandler(final OnUnitySdkDismissed callback) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                BugReporting.setOnInvokeCallback(new OnInvokeCallback() {
                    @Override
                    public void onInvoke() {
                        BugReporting.setOnDismissCallback(new OnSdkDismissCallback() {
                            @Override
                            public void call(DismissType dismissType, ReportType reportType) {
                                String dismissTypeMap = "";
                                String reportTypeMap = "";

                                if (dismissType == DismissType.SUBMIT) {
                                    dismissTypeMap = "submit";
                                } else if (dismissType == DismissType.ADD_ATTACHMENT) {
                                    dismissTypeMap = "add_attachment";
                                } else if (dismissType == DismissType.CANCEL) {
                                    dismissTypeMap = "cancel";
                                }

                                if (reportType == ReportType.BUG) {
                                    reportTypeMap = "bug";
                                } else if (reportType == ReportType.FEEDBACK) {
                                    reportTypeMap = "feedback";
                                } else if (reportType == ReportType.OTHER) {
                                    reportTypeMap = "not_available";
                                }

                                callback.onSdkDismissed(dismissTypeMap, reportTypeMap);
                            }
                        });
                    }
                });
            }
        });

    }

    public static void identifyUserWithEmail(final String email, final String name) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.identifyUser(name, email);
            }
        });
    }

    public static void logOut() {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Instabug.logoutUser();
            }
        });
    }

    public static void setWelcomeMessageMode(final int mode) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                switch (mode) {
                    case 0:
                        Instabug.setWelcomeMessageState(WelcomeMessage.State.LIVE);
                        break;
                    case 1:
                        Instabug.setWelcomeMessageState(WelcomeMessage.State.BETA);
                        break;
                    case 2:
                        Instabug.setWelcomeMessageState(WelcomeMessage.State.DISABLED);
                }
            }
        });
    }

    public static void sendScreenshotToNativeSDK(final String filePath, final String sceneName) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                Log.d("REPRO", "Screenshot to native SDK");
                Bitmap bitmap = BitmapFactory.decodeFile(filePath);
                Log.d("REPRO", bitmap + "");
                try {
                    Method method = getMethod(Class.forName("com.instabug.library.Instabug"), "reportScreenChange", Bitmap.class, String.class);
                    if (method != null) {
                        method.invoke(null, bitmap, sceneName);
                    }
                } catch (ClassNotFoundException e) {
                    e.printStackTrace();
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                } catch (InvocationTargetException e) {
                    e.printStackTrace();
                }
            }
        });
    }


    @Deprecated
    public static void showWelcomeMessageWithMode(final int welcomeMessageMode) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                WelcomeMessage.State state = WelcomeMessage.State.DISABLED;
                switch (welcomeMessageMode) {
                    case 0:
                        state = WelcomeMessage.State.LIVE;
                        break;
                    case 1:
                        state = WelcomeMessage.State.BETA;
                        break;
                    case 2:
                        state = WelcomeMessage.State.DISABLED;
                        break;
                }
                Instabug.showWelcomeMessage(state);
            }
        });
    }

    public static void setAttachmentTypesEnabled(final boolean screenshot, final boolean extraScreenshot, final boolean galleryImage, final boolean screenRecording) {
        MainThreadHandler.runOnMainThread(new Runnable() {
            @Override
            public void run() {
                BugReporting.setAttachmentTypesEnabled(screenshot, extraScreenshot, galleryImage, screenRecording);
            }
        });
    }

    private static int parseReportType(String reportType) {
        int reportTypeEnum = BugReporting.ReportType.BUG;
        switch (reportType) {
            case "Bug":
                reportTypeEnum = BugReporting.ReportType.BUG;
                break;
            case "Question":
                reportTypeEnum = BugReporting.ReportType.QUESTION;
                break;
            case "Feedback":
                reportTypeEnum = BugReporting.ReportType.FEEDBACK;
                break;
        }
        return reportTypeEnum;
    }
}
