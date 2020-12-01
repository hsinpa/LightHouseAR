using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Hsinpa.View;

namespace LightHouse.Edit {
    public class InputEditMode : MonoBehaviour
    {
        private GameObject _targetObject;

        private EditHeaderView editHeaderView;

        public enum Mode { Idle, Translate, Rotation }
        public Mode _mode;

        InputEditTranslate _inputEditTranslate;
        InputEditRotate _inputEditRotate;

        public void SetUp(Camera p_camera, EditHeaderView editHeaderView)
        {
            this.editHeaderView = editHeaderView;
            _mode = Mode.Idle;
            _inputEditTranslate = new InputEditTranslate();
            _inputEditRotate = new InputEditRotate(p_camera);

            RegisterHeaderBtnEvent();
        }

        public void SetTargetAnchor(GameObject p_targetAnchor) {
            this._targetObject = p_targetAnchor;
        }

        public void OnUpdate()
        {
            if (_mode == Mode.Translate)
                _inputEditTranslate.OnUpdate();

            if (_mode == Mode.Rotation)
                _inputEditRotate.OnUpdate();
        }

        private void RegisterHeaderBtnEvent() {

            this.editHeaderView.SetOptionEvent(
            moreInfoBtnEvent: (Button btn) =>
            {
                _mode = Mode.Idle;
            },
            translateBtnEvent : (Button btn) =>
            {
                if (_targetObject != null)
                    _inputEditTranslate.SetUp(this._targetObject);

                _mode = Mode.Translate;
            },
            rotationBtnEvent: (Button btn) =>
            {
                if (_targetObject != null)
                    _inputEditRotate.SetUp(this._targetObject);

                _mode = Mode.Rotation;
            }
            );
        }

    }
}