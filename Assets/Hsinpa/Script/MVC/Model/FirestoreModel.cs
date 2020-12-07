using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase;
using System.Threading.Tasks;
using System.Linq;
using Hsinpa.CloudAnchor;

namespace Hsinpa.Model {
    public class FirestoreModel
    {
        #region Event
        public System.Action OnInit;
        public System.Action<List<DocumentSnapshot>> OnFireBaseDocEvent;
        #endregion

        private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
        private bool isValid => dependencyStatus == DependencyStatus.Available;
        private List<ListenerRegistration> listenerRegistrationsList = new List<ListenerRegistration>();

        FirebaseFirestore db => FirebaseFirestore.DefaultInstance;

        public FirestoreModel()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
                dependencyStatus = task.Result;

                Debug.Log("FireStoreUtility: " + dependencyStatus);

                if (dependencyStatus != DependencyStatus.Available)
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
                else
                {
                    if (OnInit != null)
                        OnInit();
                }
            });
        }

        public void ListenToCollection(string collection)
        {

            var animalRef = db.Collection(collection);
            var listenReg = animalRef.Listen(snapshot => {
                Debug.Log("Callback received document snapshot.");

                foreach (DocumentSnapshot documentSnapShot in snapshot.Documents)
                {
                    if (!documentSnapShot.Exists) continue;
                    Dictionary<string, object> city = documentSnapShot.ToDictionary();
                    foreach (KeyValuePair<string, object> pair in city)
                    {
                        Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                    }
                }

                if (OnFireBaseDocEvent != null)
                    OnFireBaseDocEvent(snapshot.Documents.ToList());
            });

            listenerRegistrationsList.Add(listenReg);
        }


        public CollectionReference GetRawCollection(string collection)
        {
            if (!isValid) return null;

            return db.Collection(collection);
        }

        public async Task<List<DocumentSnapshot>> RunStoreQuery(Query p_query)
        {
            if (!isValid) return null;

            QuerySnapshot colSnapshot = await p_query.GetSnapshotAsync();

            return colSnapshot.Documents.ToList();
        }

        public async Task<List<DocumentSnapshot>> GetOnceCollection(string collection)
        {
            if (!isValid) return null;

            var animalRef = db.Collection(collection);

            QuerySnapshot colSnapshot = await animalRef.GetSnapshotAsync();
            //animalRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            //{
            //    QuerySnapshot colSnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapShot in colSnapshot.Documents)
            {
                if (documentSnapShot.Exists)
                {
                    Debug.Log(string.Format("Document data for {0} document:", documentSnapShot.Id));

                    //Dictionary<string, object> city = documentSnapShot.ToDictionary();
                    //foreach (KeyValuePair<string, object> pair in city)
                    //{
                    //    Debug.Log(string.Format("{0}: {1}", pair.Key, pair.Value));
                    //}
                }
            }

            return colSnapshot.Documents.ToList();
        }

        public async Task SaveAnchorData(CloudAnchorFireData fireData) {
            DocumentReference docRef = db.Collection(GeneralFlag.Firestore.CloudAnchorCol).Document(fireData._id);
            await docRef.SetAsync(fireData);
        }

        public void UnRegisterAll()
        {
            foreach (var listenReg in listenerRegistrationsList)
                listenReg.Stop();

            listenerRegistrationsList.Clear();
            db.App.Dispose();
        }
    }
}