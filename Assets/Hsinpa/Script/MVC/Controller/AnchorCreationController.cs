using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Input;
using Hsinpa.CloudAnchor;
using UnityEngine.UI;
using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using Hsinpa.View;

namespace Hsinpa.Controller {
    public class AnchorCreationController : ObserverPattern.Observer
    {
        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        [SerializeField]
        private EditHeaderView editHeaderView;

        [SerializeField]
        private LightHouseAnchorManager _lightHouseAnchorManager;

        [SerializeField]
        private Transform anchorWorldHolder;

        private LightHouseAnchorMesh _currentSpawnObj;
        private LightHouseAnchorView _lightHouseAnchorView;

        // Start is called before the first frame update
        void Start()
        {
            _lightHouseAnchorView = GetComponent<LightHouseAnchorView>();
            _raycastInputHandler.OnInputEvent += OnInputEvent;
            //SaveBtn.onClick.AddListener(OnSaveBtnClick);

            _lightHouseAnchorManager.OnCreateProgressUpdate += (float progress) =>
            {
                editHeaderView.SetProgressTxt("Scan Progress : " + progress);
            };

            _lightHouseAnchorManager.OnAnchorIsLocated += OnAnchorIsLocated;

            SetHeaderView();
        }

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.OnAnchorEditBack:
                    {
                        _currentSpawnObj = null;
                        SetHeaderView();
                    }
                    break;
            }
        }


        private void SetHeaderView() {

            editHeaderView.SetHomeEvent("Home", () =>
            {
                Debug.Log("Back to Menu, Which is not ready yet");
            });

            editHeaderView.DisplayOption(false);

            _raycastInputHandler.ResetRaycaster();
        }

        private void OnInputEvent(RaycastInputHandler.InputStruct inputStruct)
        {
            Debug.Log("inputStruct.inputType " + inputStruct.inputType);
            Debug.Log("raycastPosition " + inputStruct.raycastPosition);

            if (inputStruct.inputType == RaycastInputHandler.InputType.SingleTap) {
                SpawnObjectOnPos(inputStruct.raycastPosition);
            }

            //if (inputStruct.inputType == RaycastInputHandler.InputType.DoubleTap)
            //    canvasHolder.gameObject.SetActive(!canvasHolder.gameObject.activeSelf);
        }

        private void SpawnObjectOnPos(Vector3 position)
        {
            position.y = position.y + _lightHouseAnchorManager.AnchoredObjectPrefab.meshBounds.extents.y * _lightHouseAnchorManager.AnchoredObjectPrefab.transform.localScale.x;

            if (_currentSpawnObj == null)
            {
                _currentSpawnObj = _lightHouseAnchorManager.SpawnNewAnchoredObject(position, Quaternion.identity).GetComponent<LightHouseAnchorMesh>();
                _lightHouseAnchorView.RegisterNewAnchorMesh(_currentSpawnObj);
                _currentSpawnObj.transform.SetParent(anchorWorldHolder);
            } else {
                _lightHouseAnchorManager.MoveAnchoredObject(_currentSpawnObj.gameObject, position, Quaternion.identity);
            }
        }

        private void OnAnchorIsLocated(AnchorLocatedEventArgs arg) {
            if (arg.Status == LocateAnchorStatus.Located) {
                var pose = arg.Anchor.GetPose();
                _lightHouseAnchorManager.SpawnNewAnchoredObject(pose.position, pose.rotation);
            }
        }
    }
}