using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{

    public class Layer {
        public const int Anchor = 1 << 9;
    }

    public class Firestore {
        public const string CloudAnchorCol = "CloudAnchors";
        public const string ProjectID_CA = "project_id";
        public const string UserID_CA = "user_id";
        public const string Latitube_CA = "latitube";
        public const string Longitube_CA = "longitube";
        public const string Session_CA = "session";
        public const string Tag_CA = "tag";


    }

    public class FirestoreFake
    {
        public const string ProjectID_FAKE = "Test";
        public const string SESSION_FAKE = "FakeLocation_01";
    }
}
