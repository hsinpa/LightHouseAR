using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Model;
using System.Threading.Tasks;

namespace Hsinpa.CloudAnchor
{
    public class LightHouseAnchorView : MonoBehaviour
    {
        [SerializeField]
        private LightHouseAnchorManager lightHouseAnchorManager;

        [SerializeField, Range(1, 10)]
        private int numToMake = 3;

        [SerializeField, Range(1, 30)]
        private int rangeToSearch = 10;

        private int locatedCount = 0;

        private List<string> anchorIds = new List<string>();

        private AnchorLocateCriteria _anchorLocateCriteria;
        private CloudSpatialAnchorWatcher _cloudWatcher;

        // Start is called before the first frame update
        void Start()
        {
            SetUp(lightHouseAnchorManager);
        }

        public void SetUp(LightHouseAnchorManager LightHouseAnchorManager) {
            LightHouseAnchorManager.CloudManager.AnchorLocated += CloudManager_AnchorLocated;
            LightHouseAnchorManager.CloudManager.LocateAnchorsCompleted += CloudManager_LocateAnchorsCompleted;

            _anchorLocateCriteria = lightHouseAnchorManager.SetAnchorCriteria(new string[0], LocateStrategy.AnyStrategy);
        }

        private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
        {
            Debug.LogFormat("Anchor recognized as a possible anchor {0} {1}", args.Identifier, args.Status);
            if (args.Status == LocateAnchorStatus.Located)
            {
                UnityDispatcher.InvokeOnAppThread(() =>
                {
                    locatedCount++;
                    var currentCloudAnchor = args.Anchor;
                    Pose anchorPose = Pose.identity;

#if UNITY_ANDROID || UNITY_IOS
                    anchorPose = currentCloudAnchor.GetPose();
#endif
                    var spawnObject = lightHouseAnchorManager.SpawnNewAnchoredObject(anchorPose.position, anchorPose.rotation);

                    _ = DoNeighboringPassAsync(spawnObject.GetComponent<CloudSpatialAnchor>());
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
    }
}