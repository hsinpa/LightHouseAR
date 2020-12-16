﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFlag
{

    public class Layer {
        public const int Plane = 1 << 8;
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

    public enum AnchorType { 
        AnchorMain,
        AnchorSub,
        Text
    }

    public class FirestoreFake
    {
        public const string ProjectID_EDITOR = "TestEDITOR";
        public const string ProjectID_FAKE = "TainanDinosaurMuseum";
        public const string SESSION_FAKE = "FakeLocation_01";
        public const string USER_ID = "admin";

    }

    public class PlayerPref {
        public const string Sessions = "playerpref@sessions";

    }

    [System.Serializable]
    public struct SessionStruct {
        public string name;
    }
}
