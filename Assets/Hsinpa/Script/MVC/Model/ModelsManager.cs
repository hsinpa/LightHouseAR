using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.Model
{
    public class ModelsManager : MonoBehaviour
    {
        public FirestoreModel firestoreModel;

        public void SetUp() {
            firestoreModel = new FirestoreModel();
        }

    }
}