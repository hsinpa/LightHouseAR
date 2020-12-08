using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View {
    public class EditHeaderView : MonoBehaviour
    {
        [SerializeField]
        private Button HomeBtn;

        [SerializeField]
        private Text progressbarTxt;

        [Header("Options")]
        [SerializeField]
        private RectTransform OptionPanel;

        [SerializeField]
        private EditHeaderButton MoreInfoBtn;

        [SerializeField]
        private EditHeaderButton TranslateBtn;

        [SerializeField]
        private EditHeaderButton RotationBtn;

        public delegate void OnOptionClickEvent(EditHeaderButton btn);

        public void SetProgressTxt(string p_progress) {
            progressbarTxt.text = p_progress;
        }

        public void DisplayOption(bool isDisplay) {
            OptionPanel.gameObject.SetActive(isDisplay);
        }

        public void SetHomeEvent(string p_homeName, System.Action HomeBtnEvent) {
            IsButtonActivate(null);
            HomeBtn.GetComponentInChildren<Text>().text = p_homeName;

            HomeBtn.onClick.RemoveAllListeners();
            HomeBtn.onClick.AddListener(() => { HomeBtnEvent(); });
        }

        public void SetOptionEvent(OnOptionClickEvent moreInfoBtnEvent, OnOptionClickEvent translateBtnEvent, OnOptionClickEvent rotationBtnEvent) {
            //MoreInfoBtn.onClick.RemoveAllListeners();
            //TranslateBtn.onClick.RemoveAllListeners();
            //RotationBtn.onClick.RemoveAllListeners();

            MoreInfoBtn.Button.onClick.AddListener(() => {  moreInfoBtnEvent(MoreInfoBtn); });
            TranslateBtn.Button.onClick.AddListener(() => { IsButtonActivate(TranslateBtn); translateBtnEvent(TranslateBtn); });
            RotationBtn.Button.onClick.AddListener(() => { IsButtonActivate(RotationBtn); rotationBtnEvent(RotationBtn); });
        }

        public void IsButtonActivate(EditHeaderButton activateBtn) {
            TranslateBtn.ActivateHint(TranslateBtn == activateBtn);
            RotationBtn.ActivateHint(RotationBtn == activateBtn);
        }

    }
}
