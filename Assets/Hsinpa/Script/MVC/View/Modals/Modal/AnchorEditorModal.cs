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
        Text semiInfoText;

        [SerializeField]
        InputField _nameField;
        public string anchorName => _nameField.text;

        [SerializeField]
        CustomDropdown tagDropDown;
        public int tagIndex => tagDropDown.index; 

        [SerializeField]
        ButtonManagerBasic confirmBtn;

        public void SetUp(string p_semiInfo, System.Action p_onSaveEvent) {
            semiInfoText.text = p_semiInfo;

            confirmBtn.clickEvent.RemoveAllListeners();
            confirmBtn.clickEvent.AddListener(() => {
                p_onSaveEvent();
                Modals.instance.Close();
            });
        }
    }
}