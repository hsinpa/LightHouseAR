using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Model;
using System.Threading.Tasks;
using System.Linq;

namespace Hsinpa.CloudAnchor
{
    public class LightHouseAnchorView : ObserverPattern.Observer
    {
        [SerializeField]
        private LightHouseAnchorManager lightHouseAnchorManager;

        [SerializeField, Range(1, 10)]
        private int numToMake = 5;

        [SerializeField, Range(1, 30)]
        private int rangeToSearch = 15;

        [SerializeField]
        private Transform anchorWorldHolder;

        private List<CloudAnchorFireData> anchorObjs = new List<CloudAnchorFireData>();
        private List<LightHouseAnchorMesh> anchorMeshList = new List<LightHouseAnchorMesh>();
        
        private AnchorLocateCriteria _anchorLocateCriteria;
        private CloudSpatialAnchorWatcher _cloudWatcher;
        private Model.FirestoreModel _fireStoreModel;
        private int anchorFoundLength;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        _ = SetUp(lightHouseAnchorManager);
                    }
                    break;
            }
        }

        public async Task SetUp(LightHouseAnchorManager LightHouseAnchorManager) {
            _fireStoreModel = LighthouseAR.Instance.modelManager.firestoreModel;

            LightHouseAnchorManager.CloudManager.AnchorLocated += CloudManager_AnchorLocated;
            LightHouseAnchorManager.CloudManager.LocateAnchorsCompleted += CloudManager_LocateAnchorsCompleted;

            anchorObjs = await LoadAnchorPoints();

            if (anchorObjs != null && anchorObjs.Count > 0) {
                string[] anchorIds = anchorObjs.FindAll(x => x.tag == (int)GeneralFlag.AnchorType.Position).
                                                Select(x => x._id).ToArray();

                _anchorLocateCriteria = lightHouseAnchorManager.SetAnchorCriteria(anchorIds, LocateStrategy.AnyStrategy);

                await lightHouseAnchorManager.CloudManager.StartSessionAsync();

                _cloudWatcher = lightHouseAnchorManager.CreateWatcher(_anchorLocateCriteria);
            }
        }

        private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
        {
            Debug.LogFormat("Anchor recognized as a possible anchor {0} {1}", args.Identifier, args.Status);
            if (args.Status == LocateAnchorStatus.Located)
            {
                UnityDispatcher.InvokeOnAppThread(() =>
                {
                    var currentCloudAnchor = args.Anchor;
                    Pose anchorPose = Pose.identity;

                    anchorFoundLength++;

#if UNITY_ANDROID || UNITY_IOS
                    anchorPose = currentCloudAnchor.GetPose();
#endif
                    var spawnObject = lightHouseAnchorManager.SpawnNewAnchoredObject(anchorPose.position, anchorPose.rotation);
                    var cloudSpatialAnchor = spawnObject.GetComponent<CloudSpatialAnchor>();
                    LightHouseAnchorMesh anchorMesh = spawnObject.GetComponent<LightHouseAnchorMesh>();


                    anchorMesh.CloudAnchorFireData = anchorObjs.Find(x => x._id == cloudSpatialAnchor.Identifier);
                    RegisterNewAnchorMesh(anchorMesh);

                    if (anchorFoundLength == 1)
                    {
                        _ = DoNeighboringPassAsync(cloudSpatialAnchor);
                    }
                });
            }
        }

        private async Task DoNeighboringPassAsync(CloudSpatialAnchor cloudSpatialAnchor)
        {
            await lightHouseAnchorManager.CloudManager.StartSessionAsync();
            _anchorLocateCriteria = lightHouseAnchorManager.SetNearbyAnchor(_anchorLocateCriteria, cloudSpatialAnchor, rangeToSearch, numToMake);
            //locatedCount = 0;
            _cloudWatcher = lightHouseAnchorManager.CreateWatcher(_anchorLocateCriteria);
        }

        private void CloudManager_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
        {
            // OnCloudLocateAnchorsCompleted(args);
        }

        public bool RegisterNewAnchorMesh(LightHouseAnchorMesh anchorMesh) {
            int i = anchorMeshList.Count(x => x.CloudAnchorFireData._id == anchorMesh.CloudAnchorFireData._id);

            if (i < 0) {
                anchorMeshList.Add(anchorMesh);
                anchorMesh.transform.SetParent(anchorWorldHolder);
            }

            return i < 0;
        }

        public bool RemoveAnchorMesh(string anchorID) {
            int i = anchorMeshList.Count(x => x.CloudAnchorFireData._id == anchorID);

            if (i >= 0) {
                var deleteMesh = anchorMeshList[i];
                anchorMeshList.RemoveAt(i);
                Utility.UtilityMethod.SafeDestroy(deleteMesh);
            }

            return i >= 0;
        }

        private async Task<List<CloudAnchorFireData>> LoadAnchorPoints() {
            var rawCollection = this._fireStoreModel.GetRawCollection(GeneralFlag.Firestore.CloudAnchorCol);
            var query = rawCollection.WhereEqualTo(GeneralFlag.Firestore.ProjectID_CA, GeneralFlag.FirestoreFake.ProjectID_FAKE);
            var queryResult = await this._fireStoreModel.RunStoreQuery(query);

            return queryResult.Select(x => x.ConvertTo<CloudAnchorFireData>()).ToList();
        }
    }
}