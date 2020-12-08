using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;
using Hsinpa.Input;
using System.Threading.Tasks;
using Microsoft.Azure.SpatialAnchors.Unity;
using Hsinpa.CloudAnchor;
using Microsoft.Azure.SpatialAnchors;
using Firebase.Firestore;

namespace Hsinpa.Controller
{
    public class AnchorEditController : ObserverPattern.Observer
    {

        [SerializeField]
        private EditHeaderView editHeaderView;

        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        [SerializeField]
        private LightHouseAnchorManager _lightHouseAnchorManager;

        private LightHouseAnchorMesh selectedAnchorObj;

        private Model.FirestoreModel _fireStoreModel;
        private LightHouseAnchorView _lightHouseAnchorView;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        Init();

                        Debug.Log(UnityEngine.Input.location.isEnabledByUser);

                        LocationService.GetGPS(this, true, null);
                    }
                    break;


                case EventFlag.Event.OnAnchorClick:
                {
                    OnAnchorObjClick((LightHouseAnchorMesh)p_objects[0]);
                }
                break;
            }
        }

        private void Init()
        {
            _lightHouseAnchorView = GetComponent<LightHouseAnchorView>();
            _fireStoreModel = LighthouseAR.Instance.modelManager.firestoreModel;
            editHeaderView.SetOptionEvent(OnMoreInfoClick, OnTranslationClick, OnRotationClick);
        }

        private void OnAnchorObjClick(LightHouseAnchorMesh anchorObject)
        {
            selectedAnchorObj = anchorObject;

            editHeaderView.SetHomeEvent("Back", OnBackBtnClick);
            editHeaderView.DisplayOption(true);
        }

        private void OnBackBtnClick() {
            LighthouseAR.Instance.Notify(EventFlag.Event.OnAnchorEditBack);
            selectedAnchorObj = null;
        }

        private void OnMoreInfoClick(EditHeaderButton btn) {
            var anchorInfoModal = Modals.instance.OpenModal<AnchorEditorModal>();

            LocationService.GetGPS(this, true, (LocationService.LocationInfo info) => {

                string project_name = "Lighthouse";
                string semiMsg = string.Format("Project name: {0}, Longitude: {1}; Latitude: {2}", project_name, info.longitude, info.latitude);

                anchorInfoModal.SetUp(semiMsg, selectedAnchorObj.CloudAnchorFireData.name, () => {
                    OnSaveBtnClick(info, anchorInfoModal.tagIndex);
                }, () => {
                    anchorInfoModal.Show(false);
                    var dialogue = Modals.instance.OpenModal<DialogueModal>();

                    dialogue.SetDialogue(StringDataset.EditAnchor.DeleteAnchorTitle, StringDataset.EditAnchor.DeleteAnchorContent, new string[] {
                            StringDataset.Dialogue.Confirm, StringDataset.Dialogue.Cancel
                        }, (int index) => {

                            anchorInfoModal.Show(true);

                            if (index == 0) {
                                Modals.instance.Close();
                                OnAnchorRemoveClick();
                            }
                        });

                });
            });
        }

        private void OnAnchorRemoveClick() {
            if (selectedAnchorObj != null) {
                _ = _fireStoreModel.DeleteCollection(GeneralFlag.Firestore.CloudAnchorCol, selectedAnchorObj.CloudAnchorFireData._id);
                _ = _lightHouseAnchorManager.RemoveCloudAnchor(selectedAnchorObj.CloudNativeAnchor.CloudAnchor);
                _lightHouseAnchorView.RemoveAnchorMesh(selectedAnchorObj.CloudAnchorFireData._id);
                OnBackBtnClick();
            }
        }

        private void OnTranslationClick(EditHeaderButton btn)
        {

        }

        private void OnRotationClick(EditHeaderButton btn)
        {

        }

        private async void OnSaveBtnClick(LocationService.LocationInfo info, GeneralFlag.AnchorType anchorType)
        {
            if (selectedAnchorObj == null) return;

            CloudNativeAnchor cloudNativeAnchor = selectedAnchorObj.GetComponent<CloudNativeAnchor>();
            await _lightHouseAnchorManager.SaveCurrentObjectAnchorToCloudAsync(cloudNativeAnchor);

            Debug.Log("CloudAnchor.Identifier " + cloudNativeAnchor.CloudAnchor.Identifier);

            CloudAnchorFireData fireData = new CloudAnchorFireData() {
                _id = cloudNativeAnchor.CloudAnchor.Identifier,
                geopoint = new GeoPoint(info.latitude, info.longitude),
                name = System.Guid.NewGuid().ToString(),
                project_id = GeneralFlag.FirestoreFake.ProjectID_FAKE,
                user_id = GeneralFlag.FirestoreFake.USER_ID,
                session = GeneralFlag.FirestoreFake.SESSION_FAKE,
                tag = (int)anchorType
            };

            await _fireStoreModel.SaveAnchorData(GeneralFlag.Firestore.CloudAnchorCol, fireData);

            selectedAnchorObj = null;
        }

    }
}