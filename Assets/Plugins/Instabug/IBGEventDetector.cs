using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UnityEngine.UI;

namespace Plugins.Instabug
{
    public class IBGEventDetector : MonoBehaviour, IPointerClickHandler , IDragHandler, IScrollHandler
    {
#if (UNITY_IOS && !UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern void Instabug_recordUserStep(string stepName, string objectName);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
        private static AndroidJavaObject jInstabug;
#endif
        void Start()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            jInstabug = new AndroidJavaClass(IBGConstants.JAVA_INSTABUGUNITYPLUGIN_CLASS);
#endif
            addPhysicsRaycaster();
            addPhysics2DRaycaster();
        }

        void addPhysicsRaycaster()
        {
            PhysicsRaycaster physicsRaycaster = UnityEngine.Object.FindObjectOfType<PhysicsRaycaster>();
            if (physicsRaycaster == null)
            {
                Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
            }
        }
        void addPhysics2DRaycaster()
        {
            Physics2DRaycaster physicsRaycaster = UnityEngine.Object.FindObjectOfType<Physics2DRaycaster>();
            if (physicsRaycaster == null)
            {
                Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
            }
        }
        public void OnDrag(PointerEventData eventData)
        {
            string objectName = eventData.pointerCurrentRaycast.gameObject.name;
            if (objectName != null)
            {
                Debug.Log("Drag happened on object: " + objectName);
#if (UNITY_IOS && !UNITY_EDITOR)
                Instabug_recordUserStep("swipe", objectName);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                jInstabug.CallStatic("addStepWithinScene", "drag", objectName);
#endif
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            string objectName = eventData.pointerCurrentRaycast.gameObject.name;
            if (objectName != null)
            {
                Debug.Log("Click happened on object: " + objectName);
#if (UNITY_IOS && !UNITY_EDITOR)
                Instabug_recordUserStep("tap", objectName); 
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                jInstabug.CallStatic("addStepWithinScene", "Tap", objectName);
#endif
            }

        }

        public void OnScroll(PointerEventData eventData)
        {
            string objectName = eventData.pointerCurrentRaycast.gameObject.name;
            if (objectName != null)
            {
                Debug.Log("Scroll happened on object: " + objectName);
#if (UNITY_IOS && !UNITY_EDITOR)
                Instabug_recordUserStep("scroll", objectName);
#elif (UNITY_ANDROID && !UNITY_EDITOR)
                jInstabug.CallStatic("addStepWithinScene", "scroll", objectName);
#endif
            }
        }
    }
}
