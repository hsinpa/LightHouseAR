using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.CloudAnchor
{
    public class LightHouseAnchorDisplay : MonoBehaviour
    {
        [SerializeField]
        private LightHouseAnchorManager LightHouseAnchorManager;

        [SerializeField, Range(1, 10)]
        private int numToMake = 3;

        private int locatedCount = 0;

        private List<string> anchorIds = new List<string>();

        // Start is called before the first frame update
        void Start()
        {
            SetUp(LightHouseAnchorManager);
        }

        public void SetUp(LightHouseAnchorManager LightHouseAnchorManager) {
            LightHouseAnchorManager.CloudManager.AnchorLocated += CloudManager_AnchorLocated;
            LightHouseAnchorManager.CloudManager.LocateAnchorsCompleted += CloudManager_LocateAnchorsCompleted;
        }

        private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
        {
            Debug.LogFormat("Anchor recognized as a possible anchor {0} {1}", args.Identifier, args.Status);
            if (args.Status == LocateAnchorStatus.Located)
            {
                UnityDispatcher.InvokeOnAppThread(() =>
                {
                    locatedCount++;
                    currentCloudAnchor = args.Anchor;
                    Pose anchorPose = Pose.identity;

#if UNITY_ANDROID || UNITY_IOS
                    anchorPose = currentCloudAnchor.GetPose();
#endif

                });
            }
        }

        private void CloudManager_LocateAnchorsCompleted(object sender, LocateAnchorsCompletedEventArgs args)
        {
            // OnCloudLocateAnchorsCompleted(args);
        }
    }
}