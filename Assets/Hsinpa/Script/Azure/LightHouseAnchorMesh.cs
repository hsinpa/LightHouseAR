using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.CloudAnchor
{
    public class LightHouseAnchorMesh : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        public Bounds meshBounds => meshFilter.sharedMesh.bounds;

        public CloudAnchorFireData CloudAnchorFireData = CloudAnchorFireData.GetDefault();

        public bool dataIsSet => (CloudAnchorFireData != null && CloudAnchorFireData.project_id != GeneralFlag.FirestoreFake.ProjectID_EDITOR);
    }
}
