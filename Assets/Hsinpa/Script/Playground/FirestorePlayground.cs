using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.Model;

public class FirestorePlayground : MonoBehaviour
{
    FirestoreModel firestoreModel;

    // Start is called before the first frame update
    void Start()
    {
        firestoreModel = new FirestoreModel();

        firestoreModel.OnInit += OnFirebaseReady;
    }

    void OnFirebaseReady() {
        FireRequest();
    }

    async void FireRequest() {
        var rawCollection = this.firestoreModel.GetRawCollection(GeneralFlag.Firestore.CloudAnchorCol);
        var query = rawCollection.WhereEqualTo(GeneralFlag.Firestore.ProjectID_CA, GeneralFlag.FirestoreFake.ProjectID_FAKE);
        var queryResult = await this.firestoreModel.RunStoreQuery(query);

        Debug.Log("FireRequest count " + queryResult.Count);
    }
}
