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

        public CloudAnchorFireData CloudAnchorFireData;
        public bool dataIsSet => CloudAnchorFireData != null;
    }
}
