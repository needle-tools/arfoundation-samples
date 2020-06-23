using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif

public class TouchVis : MonoBehaviour
{   
    public bool enhancedTouch = false;
    public Text touchCount;
    public Text touchData;

    void Awake() {
#if UNITY_NEW_INPUT_SYSTEM
        if(enhancedTouch)
            EnhancedTouchSupport.Enable();
#endif
        DontDestroyOnLoad(this.gameObject);
    }

    private void LateUpdate()
    {
        // "Old" Input
        touchCount.text = "Touches: " + Input.touchCount;
        var s = "";
        for(int i = 0; i < Input.touchCount; i++) {
            var t = Input.GetTouch(i);
            s += "#" + i + ": id: " + t.fingerId + ", pos: " + t.position + "\n";
            s += "   phase: " + t.phase + ", tap count: " + t.tapCount + ", raw: " + t.rawPosition + "\n";
            s += "   type: " + t.type + ", radius: " + t.radius + ", var: " + t.radiusVariance + "\n";
            s += "   pressure: " + t.pressure + ", max p: " + t.maximumPossiblePressure + "\n";
            // s += "   delta pos: " + t.deltaPosition + ", delta time: " + t.deltaTime + "\n";
        }
        touchData.text = s;

        // "New" Input
#if UNITY_NEW_INPUT_SYSTEM
        var touchScreen = Touchscreen.current;
        if (touchScreen != null && touchScreen.enabled)
        {
            s += "\nNew Input\n";
            s += "Touches: " + touchScreen.touches.Count + "\n";
            int k = 0;
            foreach (var touch in touchScreen.touches)
            {
                var phase = touch.phase.ReadValue();
                if(phase == UnityEngine.InputSystem.TouchPhase.None) continue;
                s += "#" + k + ": id: " + touch.touchId + ", pos: " + touch.position.ReadValue() + "\n";
                s += "   phase: " + touch.phase.ReadValue() + ", tap count: " + touch.tapCount.ReadValue() + "\n";
                s += "   duration: " + (Time.realtimeSinceStartup - touch.startTime.ReadValue()) + "\n";
                k++;
            }
            touchData.text = s;
        }

        if(enhancedTouch) {
            s += "\nEnhanced Touch API\n";
            s += "Touches: " + ETouch.activeTouches.Count + "\n";
            int k = 0;
            foreach (var touch in ETouch.activeTouches) {
                if(!touch.valid) continue;
                s += "#" + k + ": id: " + touch.touchId + ", pos: " + touch.screenPosition + "\n";
                s += "   phase: " + touch.phase + ", tap count: " + touch.tapCount + "\n";
                s += "   duration: " + (Time.realtimeSinceStartup - touch.startTime) + "\n";
                k++;
            }
            s += "Fingers: " + ETouch.activeFingers.Count + "\n";
            k = 0;
            foreach (var finger in ETouch.activeFingers) {
                // if(!finger.isActive continue;
                s += "#" + k + ": id: " + finger.index + ", pos: " + finger.screenPosition + "\n";
                k++;
            }
            
            touchData.text = s;
        }
#endif
    }
}
