using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefHelper
{
    public static void SaveSessionData(GeneralFlag.SessionStruct[] sessions)
    {
        PlayerPrefs.SetString(GeneralFlag.PlayerPref.Sessions, JsonHelper.ToJson(sessions));
        PlayerPrefs.Save();
    }

    public static GeneralFlag.SessionStruct[] GetSessionData()
    {
        string sessionRaw = PlayerPrefs.GetString(GeneralFlag.PlayerPref.Sessions);

        if (string.IsNullOrEmpty(sessionRaw))
            return new GeneralFlag.SessionStruct[0];

        return JsonHelper.FromJsonWithWrapper<GeneralFlag.SessionStruct>(sessionRaw);
    }

}
