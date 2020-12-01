using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using UnityEngine.UI;
using Hsinpa.Input;
using System.Runtime.InteropServices;

namespace Hsinpa.Controller
{
    public class AnchorEditController : ObserverPattern.Observer
    {

        [SerializeField]
        private EditHeaderView editHeaderView;

        [SerializeField]
        private RaycastInputHandler _raycastInputHandler;

        private GameObject selectedAnchorObj;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            switch (p_event)
            {
                case EventFlag.Event.GameStart:
                    {
                        Init();
                    }
                    break;


                case EventFlag.Event.OnAnchorClick:
                {
                    OnAnchorObjClick((GameObject)p_objects[0]);
                }
                break;
            }
        }

        private void Init()
        {
            editHeaderView.SetOptionEvent(OnMoreInfoClick, OnTranslationClick, OnRotationClick);
        }

        private void OnAnchorObjClick(GameObject anchorObject)
        {
            selectedAnchorObj = anchorObject;

            editHeaderView.SetHomeEvent("Back", OnBackBtnClick);
            editHeaderView.DisplayOption(true);
        }

        private void OnBackBtnClick() {
            LighthouseAR.Instance.Notify(EventFlag.Event.OnAnchorEditBack);
        }

        private void OnMoreInfoClick(Button btn) {
            var anchorInfoModal = Modals.instance.OpenModal<AnchorEditorModal>();
        }

        private void OnTranslationClick(Button btn)
        {

        }

        private void OnRotationClick(Button btn)
        {

        }

    }
}