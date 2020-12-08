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

        public static CloudAnchorFireData GetDefault() {
            string rID = System.Guid.NewGuid().ToString();
            return new CloudAnchorFireData()
            {
                _id = rID,
                geopoint = new GeoPoint(0, 0),
                name = rID,
                project_id = GeneralFlag.FirestoreFake.ProjectID_EDITOR,
                user_id = GeneralFlag.FirestoreFake.USER_ID,
                session = GeneralFlag.FirestoreFake.SESSION_FAKE,
                tag = 0
            };
        }
    }
}