using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsinpa.View {
    public class EditHeaderView : MonoBehaviour
    {
        [SerializeField]
        private Button HomeBtn;

        [Header("Options")]
        [SerializeField]
        private RectTransform OptionPanel;

        [SerializeField]
        private Button MoreInfoBtn;

        [SerializeField]
        private Button TranslateBtn;

        [SerializeField]
        private Button RotationBtn;

        public delegate System.Action<Button> OnOptionClickEvent();

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

            MoreInfoBtn.onClick.AddListener(() => { moreInfoBtnEvent(); });
            TranslateBtn.onClick.AddListener(() => { translateBtnEvent(); });
            RotationBtn.onClick.AddListener(() => { rotationBtnEvent(); });
        }


    }
}
