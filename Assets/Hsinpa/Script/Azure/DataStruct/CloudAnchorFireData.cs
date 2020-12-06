using Firebase.Firestore;

namespace Hsinpa.CloudAnchor { 
    [FirestoreData]
    public class CloudAnchorFireData
    {
        [FirestoreProperty]
        public string _id { get; set; }

        [FirestoreProperty]
        public GeoPoint geopoint { get; set; }

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public string project_id { get; set; }

        [FirestoreProperty]
        public string session { get; set; }

        [FirestoreProperty]
        public int tag { get; set; }

        [FirestoreProperty]
        public string user_id { get; set; }
    }
}