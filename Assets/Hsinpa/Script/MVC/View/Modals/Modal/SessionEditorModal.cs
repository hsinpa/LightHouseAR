using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using System.Linq;

namespace Hsinpa.View
{
    public class SessionEditorModal : Modal
    {
        [SerializeField]
        InputField sessionInputField;

        [SerializeField]
        Button sessionInputBtn;

        [SerializeField]
        SessionEditorTagView sessionTagPrefab;

        [SerializeField]
        Transform sessionTagHolder;

        private List<GeneralFlag.SessionStruct> sessionStructList = new List<GeneralFlag.SessionStruct>();

        private void Awake()
        {
            sessionInputBtn.onClick.AddListener(() =>
            {

                if (sessionInputField.text != "") {
                    OnSessionIputFieldEvent(sessionInputField.text);
                    sessionInputField.text = "";
                }
            });

        }

        public void SetUp() {
            sessionInputField.text = "";
            RenderSessionTag();
        }

        private void RenderSessionTag() {
            GeneralFlag.SessionStruct[] sessions = GetSessionData();
            sessionStructList = sessions.ToList();

            UtilityMethod.ClearChildObject(sessionTagHolder);

            foreach (GeneralFlag.SessionStruct sessionStruct in sessions) {
                InsertSert(sessionStruct);
            }
        }

        private void OnSessionIputFieldEvent(string session_name) {
            GeneralFlag.SessionStruct newSession = new GeneralFlag.SessionStruct();
            newSession.name = session_name;
            sessionStructList.Add(newSession);

            InsertSert(newSession);
            SaveToPlayPref(sessionStructList.ToArray());
        }

        private void InsertSert(GeneralFlag.SessionStruct sessionStruct) {
            var sessionObj = UtilityMethod.CreateObjectToParent(sessionTagHolder, sessionTagPrefab.gameObject);
            sessionObj.name = sessionStruct.name;
            SessionEditorTagView sessionView = sessionObj.GetComponent<SessionEditorTagView>();
            sessionView.SetUp(sessionStruct.name, (GameObject sessionObject) => {

                int sIndex = sessionStructList.FindIndex(x => x.name == sessionObject.name);

                if (sIndex >= 0)
                {
                    sessionStructList.RemoveAt(sIndex);

                    UtilityMethod.SafeDestroy(sessionObject);

                    SaveToPlayPref(sessionStructList.ToArray());
                }
            });
        }

        private void SaveToPlayPref(GeneralFlag.SessionStruct[] sessions) {
            PlayerPrefs.SetString(GeneralFlag.PlayerPref.Sessions, JsonHelper.ToJson(sessions));
            PlayerPrefs.Save();
        }

        public GeneralFlag.SessionStruct[] GetSessionData() {
            string sessionRaw = PlayerPrefs.GetString(GeneralFlag.PlayerPref.Sessions);

            if (string.IsNullOrEmpty(sessionRaw))
                return new GeneralFlag.SessionStruct[0];

            return JsonHelper.FromJsonWithWrapper<GeneralFlag.SessionStruct>(sessionRaw);
        }

    }
}