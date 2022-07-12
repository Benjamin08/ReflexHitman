# **Instabug for Unity**

Detailed bug reporting and direct in-app chat with users for mobile games made with Unity on iOS and Android! :video_game:

<img alt="Screen Shot 2022-03-09 at 12 21 43 PM" src="https://user-images.githubusercontent.com/36373152/157422399-b616d4cb-4b23-4ca1-b15f-7112ece47a3f.png" width="22%"><img alt="Screen Shot 2022-03-09 at 11 03 51 AM" src="https://user-images.githubusercontent.com/36373152/157422471-08a13308-21de-4049-874a-ae19396e1da0.png" width="76%">

Let your users seamlessly report in-game bugs and feedback, and speed up your game development cycle with Instabug, thanks to the comprehensive metadata which we capture to help you identify the cause of bugs and squash them faster.

For more info, you can visit [Instabug for Unity](https://www.instabug.com/platforms/unity "Instabug for Unity").

## Installation

1. Download [Instabug-Unity](https://github.com/Instabug/Instabug-Unity/releases "Instabug-Unity") package.
2. In your own Unity project, import the package by selecting: 
Assets → Import Package → Custom Package from Unity editor’s menu OR double-clicking Instabug-Unity.unitypackage.
3. Resolve dependencies (for Android).

## Initializing Instabug

1. Create an empty GameObject and add `Instabug.cs` script as a component to it. The script is located in: Assets → Plugins → Instabug
2. Enter your app token, which you can find on the dashboard, and choose the preferred Invocation Event from the inspector window.
3. Add **Event System** component to the newly created GameObject (only if you do not already have an EventSystem game object in the hierarchy of the current scene).

:warning: For iOS builds, you may encounter a build error on some Xcode versions. To resolve it, you can simply set **Validate Workspace** to **Yes** in the Build Settings tab under Build Options in Xcode.

## Migrating from versions prior to v10

1. Remove the Assets/Plugins directory that was previously imported.
2. Follow the [installation steps](#installation).

## Documentation

For more details about the supported APIs and how to use them, you can refer to our [docs](https://docs.instabug.com/docs/unity-overview "**docs**").

## Contact Us

If you have any questions or feedback, don’t hesitate to get in touch. You can reach out to us at any time through  [support@instabug.com](mailto:support@instabug.com).