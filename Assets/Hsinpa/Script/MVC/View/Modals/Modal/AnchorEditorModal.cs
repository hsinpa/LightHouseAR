using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Michsky.UI.ModernUIPack;

namespace Hsinpa.View
{
    public class AnchorEditorModal : Modal
    {
        [SerializeField]
        Sprite sessionDropdownSprite;

        [SerializeField]
        Text semiInfoText;

        [SerializeField]
        InputField _nameField;
        public string anchorName => _nameField.text;

        [SerializeField]
        CustomDropdown tagDropDown;
        public GeneralFlag.AnchorType tagIndex => (GeneralFlag.AnchorType) tagDropDown.index;

        [SerializeField]
        CustomDropdown sessionDropDown;
        public string selectedSessionString {
            get {
                if (tagDropDown.index < 0 || sessionDropDown.dropdownItems == null)
                    return "";

                return sessionDropDown.dropdownItems[tagDropDown.index].itemName;
            }
        }

        [SerializeField]
        ButtonManagerBasic confirmBtn;

        [SerializeField]
        ButtonManagerBasic deleteBtn;

        public void SetUp(string p_semiInfo, string anchorName, GeneralFlag.SessionStruct[] availableSessions, System.Action p_onSaveEvent, System.Action p_onRemoveEvent) {
            semiInfoText.text = p_semiInfo;
            _nameField.text = anchorName;

            confirmBtn.clickEvent.RemoveAllListeners();
            confirmBtn.clickEvent.AddListener(() => {
                p_onSaveEvent();
                Modals.instance.Close();
            });

            deleteBtn.clickEvent.RemoveAllListeners();
            deleteBtn.clickEvent.AddListener(() => {
                if (p_onRemoveEvent != null)
                    p_onRemoveEvent();
            });

            SetSessionDropdown(availableSessions);
        }

        private void SetSessionDropdown(GeneralFlag.SessionStruct[] p_availableSessions) {
            sessionDropDown.dropdownItems.Clear();
            foreach (GeneralFlag.SessionStruct session in p_availableSessions) {
                sessionDropDown.CreateNewItemFast(session.name, sessionDropdownSprite);
            }

            sessionDropDown.SetupDropdown();
        }
    }
}