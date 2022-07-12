#import <Instabug/Instabug.h>
#import <Instabug/IBGBugReporting.h>

#if __cplusplus
extern "C" {
#endif
    typedef void (*PreSendingCallbackDelegate)();
    typedef void (*PreInvocationCallbackDelegate)();
    typedef void (*PostInvocationCallbackDelegate)(IBGDismissType dismissType, IBGReportType reportType);
#if __cplusplus
}
#endif

#ifdef __cplusplus
extern "C" {
#endif
    
    void UnitySendMessage(const char *obj, const char *method, const char *msg);
    
#ifdef __cplusplus
}
#endif

void Instabug_InitializeInstabug(char *appToken, int invocationEvent) {
    SEL setPrivateApiSEL = NSSelectorFromString(@"setCrossPlatform:");
    if ([[Instabug class] respondsToSelector:setPrivateApiSEL]) {
        BOOL flag = true;
        NSNumber *enableCross = [NSNumber numberWithBool:flag];
        NSInvocation *inv = [NSInvocation invocationWithMethodSignature:[[Instabug class] methodSignatureForSelector:setPrivateApiSEL]];
        [inv setSelector:setPrivateApiSEL];
        [inv setTarget:[Instabug class]];
        [inv setArgument:&(enableCross) atIndex:2];
        [inv invoke];
    }
    [Instabug startWithToken:[NSString stringWithUTF8String:appToken]
            invocationEvents: invocationEvent];
}

void Instabug_SetAutoScreenRecordingEnabled(BOOL isEnabled) {
    IBGBugReporting.autoScreenRecordingEnabled = YES;
}

void Instabug_SetAutoScreenRecordingAudioCapturingEnabled(bool isEnabled) {
    SEL setPrivateApiSEL = NSSelectorFromString(@"setAutoScreenRecordingAudioCapturingEnabled:");
    if ([[Instabug class] respondsToSelector:setPrivateApiSEL]) {
        NSInvocation *inv = [NSInvocation invocationWithMethodSignature:[[Instabug class] methodSignatureForSelector:setPrivateApiSEL]];
        [inv setSelector:setPrivateApiSEL];
        [inv setTarget:[Instabug class]];
        [inv setArgument:&(isEnabled) atIndex:2];
        [inv invoke];
    }
}

void Instabug_SetAutoScreenRecordingDuration(int duration) {
    IBGBugReporting.autoScreenRecordingDuration = duration;
}

void Instabug_recordVisualUserStepForView(char *viewName) {
    SEL setPrivateApiSEL = NSSelectorFromString(@"logViewDidAppearEvent:");
    if ([[Instabug class] respondsToSelector:setPrivateApiSEL]) {
        NSString *strViewName = [NSString stringWithUTF8String:viewName];
        NSInvocation *inv = [NSInvocation invocationWithMethodSignature:[[Instabug class] methodSignatureForSelector:setPrivateApiSEL]];
        [inv setSelector:setPrivateApiSEL];
        [inv setTarget:[Instabug class]];
        [inv setArgument:&(strViewName) atIndex:2];
        [inv invoke];
    }
}

void Instabug_recordUserStep(char *stepName, char *objectName) {
    SEL setPrivateApiSEL = NSSelectorFromString(@"logTouchEvent:viewName:");
    if ([[Instabug class] respondsToSelector:setPrivateApiSEL]) {
        NSString *strStepName = [NSString stringWithUTF8String:stepName];
        NSString *strObjectName = [NSString stringWithUTF8String:objectName];
        NSInvocation *inv = [NSInvocation invocationWithMethodSignature:[[Instabug class] methodSignatureForSelector:setPrivateApiSEL]];
        [inv setSelector:setPrivateApiSEL];
        [inv setTarget:[Instabug class]];
        [inv setArgument:&(strStepName) atIndex:2];
        [inv setArgument:&(strObjectName) atIndex:3];
        [inv invoke];
    }
}

void Instabug_LogDebug(char *log) {
    IBGLogDebug(@"%@", [NSString stringWithUTF8String:log]);
}

void Instabug_LogVerbose(char *log) {
    IBGLogVerbose(@"%@", [NSString stringWithUTF8String:log]);
}

void Instabug_LogWarn(char *log) {
    IBGLogWarn(@"%@", [NSString stringWithUTF8String:log]);
}

void Instabug_LogInfo(char *log) {
    IBGLogInfo(@"%@", [NSString stringWithUTF8String:log]);
}

void Instabug_LogError(char *log) {
    IBGLogError(@"%@", [NSString stringWithUTF8String:log]);
}

void Instabug_Show() {
    [Instabug show];
}

void Instabug_showWithReportTypeAndOptions(int reportType, int options) {
    [IBGBugReporting showWithReportType:reportType options:options];
}

void Instabug_setBugReportingOptions(int options) {
    IBGBugReporting.bugReportingOptions = options;
}

void Instabug_Dismiss() {
    [IBGBugReporting dismiss];
}

void Instabug_AddFileAttachmentWithURL(char *url) {
    //TODO: test. not changed.
    NSString* urlString = [NSString stringWithUTF8String:url];
    [Instabug addFileAttachmentWithURL:[NSURL URLWithString:urlString]];
}

void Instabug_ClearFileAttachments() {
    //TODO: test. not changed.
    [Instabug clearFileAttachments];
}

void Instabug_SetUserData(char *userData) {
    [Instabug setUserData:[NSString stringWithUTF8String:userData]];
}

static PreSendingCallbackDelegate preSendingCallback;

#ifdef __cplusplus
extern "C" {
#endif
    
    void Instabug_SetPreSendingHandler(PreSendingCallbackDelegate callback) {
        //TODO: test. remove commented part.
        preSendingCallback = callback;
        if (preSendingCallback != nil) {
            //        [Instabug setPreSendingHandler:^() {
            //            preSendingCallback();
            //        }];
            Instabug.willSendReportHandler = ^(IBGReport* report){
                preSendingCallback();
                return report;
            };
        } else {
            NSLog(@"Callback cannot be null");
        }
    }
    
#ifdef __cplusplus
}
#endif

static PreInvocationCallbackDelegate preInvocationCallback;

#ifdef __cplusplus
extern "C" {
#endif
    
    void Instabug_SetPreInvocationHandler(PreInvocationCallbackDelegate callback) {
        
        // TODO: test. remove commented part.
        preInvocationCallback = callback;
        if (preInvocationCallback != nil) {
            //        [Instabug setPreInvocationHandler:^() {
            //            preInvocationCallback();
            //        }];
            IBGBugReporting.willInvokeHandler = ^{
                preInvocationCallback();
            };
        } else {
            NSLog(@"Callback cannot be null");
        }
    }
    
#ifdef __cplusplus
}
#endif

static PostInvocationCallbackDelegate postInvocationCallback;

#ifdef __cplusplus
extern "C" {
#endif
    
    void Instabug_SetPostInvocationHandler(PostInvocationCallbackDelegate callback) {
        
        //TODO: test. remove commented part.
        postInvocationCallback = callback;
        if (postInvocationCallback != nil) {
            //        [Instabug setPostInvocationHandler:^(IBGDismissType dismissType, IBGReportType reportType) {
            //            postInvocationCallback(dismissType, reportType);
            //        }];
            IBGBugReporting.didDismissHandler = ^(IBGDismissType dismissType, IBGReportType reportType) {
                postInvocationCallback(dismissType, reportType);
            };
        } else {
            NSLog(@"Callback cannot be null");
        }
    }
    
#ifdef __cplusplus
}
#endif

void Instabug_IdentifyUserWithEmail(char *email, char *name) {
    [Instabug identifyUserWithEmail:[NSString stringWithUTF8String:email] name:[NSString stringWithUTF8String:name]];
}

void Instabug_LogOut() {
    [Instabug logOut];
}

void Instabug_ShowWelcomeMessageWithMode(int welcomeMessageMode) {
    [Instabug showWelcomeMessageWithMode:welcomeMessageMode];
}

void Instabug_SetAttachmentTypesEnabled(BOOL screenshot, BOOL extraScreenshot, BOOL galleryImage, BOOL screenRecording) {
   IBGAttachmentType attachmentTypes = 0;
    if(screenshot) {
        attachmentTypes = IBGAttachmentTypeScreenShot;
    }
    if(extraScreenshot) {
        attachmentTypes |= IBGAttachmentTypeExtraScreenShot;
    }
    if(galleryImage) {
        attachmentTypes |= IBGAttachmentTypeGalleryImage;
    }
    if(screenRecording) {
        attachmentTypes |= IBGAttachmentTypeScreenRecording;
    }
    IBGBugReporting.enabledAttachmentTypes = attachmentTypes;
}

void Instabug_SetWelcomeMessageMode(int welcomeMessageMode) {
     [Instabug setWelcomeMessageMode:welcomeMessageMode];
}

void Instabug_AddFileAttachmentWithData(char* strData) {
    NSData *data = [[NSString stringWithUTF8String:strData] dataUsingEncoding:NSUTF8StringEncoding];
    [Instabug addFileAttachmentWithData:data];
}
