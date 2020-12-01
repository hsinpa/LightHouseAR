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
    public class AnchorEditController : MonoBehaviour
    {
        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        [SerializeField]
        private EditHeaderView editHeaderView;

        [SerializeField]
        private LightHouseAnchorManager _lightHouseAnchorManager;

        private GameObject _currentSpawnObj;

        // Start is called before the first frame update
        void Start()
        {
            _raycastInputHandler.OnInputEvent += OnInputEvent;
            //SaveBtn.onClick.AddListener(OnSaveBtnClick);

            _lightHouseAnchorManager.OnCreateProgressUpdate += (float progress) =>
            {
                editHeaderView.SetProgressTxt("Scan Progress : " + progress);
            };

            _lightHouseAnchorManager.OnAnchorIsLocated += OnAnchorIsLocated;
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
            if (_currentSpawnObj == null)
            {
                _currentSpawnObj = _lightHouseAnchorManager.SpawnNewAnchoredObject(position, Quaternion.identity);
            } else {
                _lightHouseAnchorManager.MoveAnchoredObject(_currentSpawnObj, position, Quaternion.identity);
            }
        }

        private async void OnSaveBtnClick() {
            if (_currentSpawnObj == null) return;

            //SaveBtn.interactable = false;

            CloudNativeAnchor cloudNativeAnchor = _currentSpawnObj.GetComponent<CloudNativeAnchor>();
            await _lightHouseAnchorManager.SaveCurrentObjectAnchorToCloudAsync(cloudNativeAnchor);

            Debug.Log("CloudAnchor.Identifier " + cloudNativeAnchor.CloudAnchor.Identifier);

            var criteria = _lightHouseAnchorManager.SetAnchorCriteria(new string[1] { "f7e2ae12-9214-4909-963c-f830a2a1e003" }, LocateStrategy.AnyStrategy);
            _lightHouseAnchorManager.CreateWatcher(criteria);
            //_lightHouseAnchorManager.CloudManager.StopSession();

            _currentSpawnObj = null;
            //SaveBtn.interactable = true;
        }

        private void OnAnchorIsLocated(AnchorLocatedEventArgs arg) {
            if (arg.Status == LocateAnchorStatus.Located) {
                var pose = arg.Anchor.GetPose();
                _lightHouseAnchorManager.SpawnNewAnchoredObject(pose.position, pose.rotation);
            }
        }
    }
}