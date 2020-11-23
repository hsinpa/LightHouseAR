using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Hsinpa.Input
{
    public class RaycastInputHandler : MonoBehaviour
    {
        [SerializeField]
        private GraphicRaycaster canvasRaycaster;

        [SerializeField]
        private ARRaycastManager arRaycastManager;

        [SerializeField]
        private ARCameraManager arCamera;

        [SerializeField, Range(0.05f, 1f)]
        private float doubleTapTreshold = 0.1f;
        private float timeRecord;
        private int tapCount = 0;

        private PointerEventData eventData;
        private List<RaycastResult> raycastResults = new List<RaycastResult>();
        private List<ARRaycastHit> arRaycastResults = new List<ARRaycastHit>();

        public enum InputType { SingleTap, DoubleTap };
        public System.Action<InputStruct> OnInputEvent;

        private void Start()
        {
            eventData = new PointerEventData(EventSystem.current);
        }

        public Quaternion GetFrontQuaternion(Vector3 hitPoint, Vector3 offset)
        {
            var cameraDir = (arCamera.transform.position - hitPoint).normalized;
            cameraDir.y = 0;

            var quaRot = Quaternion.LookRotation(cameraDir);
            return Quaternion.Euler(quaRot.eulerAngles + offset);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) && OnInputEvent != null)
            {
                arRaycastResults.Clear();
                if (CheckIsDoubleTabActivate())
                {
                    OnInputEvent(new InputStruct(Vector3.zero, InputType.DoubleTap));
                    return;
                }

                if (CheckNoUIIsActivate())
                {
                    return;
                }

                arRaycastManager.Raycast(UnityEngine.Input.mousePosition, arRaycastResults, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);

                if (arRaycastResults.Count > 0)
                {
                    OnInputEvent(new InputStruct(arRaycastResults[0].pose.position, InputType.SingleTap));
                }
            }
        }

        private bool CheckIsDoubleTabActivate()
        {
            if (Time.time > timeRecord)
            {
                tapCount = 1;
                timeRecord = doubleTapTreshold + Time.time;
            }
            else
            {
                tapCount++;
            }

            if (tapCount >= 2)
            {
                tapCount = 0;
                timeRecord = 0;
                return true;
            }

            return false;
        }

        private bool CheckNoUIIsActivate()
        {
            raycastResults.Clear();

            eventData.position = UnityEngine.Input.mousePosition;
            EventSystem.current.RaycastAll(eventData, raycastResults);
            return raycastResults.Count > 0;
        }


        public struct InputStruct
        {
            public Vector3 raycastPosition;
            public RaycastInputHandler.InputType inputType;

            public InputStruct(Vector3 p_pos, RaycastInputHandler.InputType p_inputType)
            {
                this.raycastPosition = p_pos;
                this.inputType = p_inputType;
            }
        }
    }
}