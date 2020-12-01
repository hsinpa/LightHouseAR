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
        private Button MoreInfoBtn;

        [SerializeField]
        private Button TranslateBtn;

        [SerializeField]
        private Button RotationBtn;

        [SerializeField]
        private Button SaveBtn;

        public delegate void OnOptionClickEvent(Button btn);

        public void SetProgressTxt(string p_progress) {
            progressbarTxt.text = p_progress;
        }

        public void DisplayOption(bool isDisplay) {
            OptionPanel.gameObject.SetActive(isDisplay);
        }

        public void SetHomeEvent(System.Action HomeBtnEvent) {
            HomeBtn.onClick.RemoveAllListeners();
            HomeBtn.onClick.AddListener(() => { HomeBtnEvent(); });
        }

        public void SetOptionEvent(OnOptionClickEvent moreInfoBtnEvent, OnOptionClickEvent translateBtnEvent, OnOptionClickEvent rotationBtnEvent) {
            MoreInfoBtn.onClick.RemoveAllListeners();
            TranslateBtn.onClick.RemoveAllListeners();
            RotationBtn.onClick.RemoveAllListeners();

            MoreInfoBtn.onClick.AddListener(() => { moreInfoBtnEvent(MoreInfoBtn); });
            TranslateBtn.onClick.AddListener(() => { translateBtnEvent(TranslateBtn); });
            RotationBtn.onClick.AddListener(() => { rotationBtnEvent(RotationBtn); });
        }


    }
}
