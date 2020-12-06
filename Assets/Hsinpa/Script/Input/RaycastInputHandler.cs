using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using LightHouse.Edit;
using Hsinpa.View;
using Utility;

namespace Hsinpa.Input
{
    public class RaycastInputHandler : MonoBehaviour
    {
        [SerializeField]
        private ARRaycastManager arRaycastManager;

        [SerializeField]
        private ARCameraManager arCamera;
        private Camera _camera;

        [SerializeField]
        private InputEditMode _lightHouseEditMode;
        public InputEditMode lightHouseEditMode => _lightHouseEditMode;

        [SerializeField]
        private EditHeaderView editHeaderView;

        [SerializeField, Range(0.05f, 1f)]
        private float doubleTapTreshold = 0.2f;
        private float timeRecord;
        private int tapCount = 0;

        private PointerEventData eventData;
        private List<RaycastResult> raycastResults = new List<RaycastResult>();
        private List<ARRaycastHit> arRaycastResults = new List<ARRaycastHit>();
        private RaycastHit[] anchorHits = new RaycastHit[1];
        private GameObject selectedAnchor;

        public enum InputType { SingleTap, DoubleTap, None };
        public System.Action<InputStruct> OnInputEvent;

        private InputStruct _inputStruct;

        private void Start()
        {
            this._camera = arCamera.GetComponent<Camera>();
            eventData = new PointerEventData(EventSystem.current);

            _lightHouseEditMode.SetUp(arCamera.GetComponent<Camera>(), editHeaderView);
            _inputStruct = new InputStruct();
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

            if (selectedAnchor != null) {

                _lightHouseEditMode.OnUpdate();

                return;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0) && OnInputEvent != null)
            {
                //EXIST AR ANCHOR Detection
                selectedAnchor = CheckCastOnExistAnchor();
                if (selectedAnchor != null) {
                    Debug.Log("Hit on anchor");
                    selectedAnchor.name = UtilityMethod.GetRandomIDString();
                    _lightHouseEditMode.SetTargetAnchor(selectedAnchor);
                    LighthouseAR.Instance.Notify(EventFlag.Event.OnAnchorClick, selectedAnchor);
                    return;
                }
                
                //UI AND ARPLANE Detection
                if (CheckIsDoubleTabActivate())
                {
                    _inputStruct.raycastPosition = Vector3.zero;
                    _inputStruct.inputType = InputType.DoubleTap;
                    OnInputEvent(_inputStruct);
                    return;
                }

                if (CheckNoUIIsActivate())
                {
                    return;
                }

                _inputStruct = CheckPlaneIsCast();
                if (_inputStruct.inputType == InputType.SingleTap)
                {
                    OnInputEvent(_inputStruct);
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

        private InputStruct CheckPlaneIsCast() {
            _inputStruct.inputType = InputType.None;

#if UNITY_EDITOR
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, anchorHits, 100, GeneralFlag.Layer.Plane);
            if (hitCount > 0) {
                _inputStruct.inputType = InputType.SingleTap;
                _inputStruct.raycastPosition = anchorHits[0].point;
            }
#else
            arRaycastResults.Clear();
            bool hasHit = arRaycastManager.Raycast(UnityEngine.Input.mousePosition, arRaycastResults, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            if (hasHit) {
                _inputStruct.inputType = InputType.SingleTap;
                _inputStruct.raycastPosition = arRaycastResults[0].pose.position;
            }
#endif

            return _inputStruct;
        }

        private GameObject CheckCastOnExistAnchor()
        {
            Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, anchorHits, 100, GeneralFlag.Layer.Anchor);

            return (hitCount >= 1) ? anchorHits[0].transform.gameObject : null;
        }

        public void ResetRaycaster() {
            selectedAnchor = null;
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