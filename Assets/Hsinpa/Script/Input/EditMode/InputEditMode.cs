using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace LightHouse.Edit {
    public class InputEditMode : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _testObject;

        [SerializeField]
        private Button showMoreInfoBtn;

        [SerializeField]
        private Button enableTranslationBtn;

        [SerializeField]
        private Button enableRotationBtn;

        public enum Mode {Idle, Translate, Rotation }
        public Mode _mode;

        InputEditTranslate _inputEditTranslate;

        private RaycastHit[] raycastHits = new RaycastHit[1];

        public void Start()
        {
            _mode = Mode.Translate;
            _inputEditTranslate = new InputEditTranslate();
            _inputEditTranslate.SetUp(_testObject);

            RegisterHeaderBtnEvent();
        }

        public void Update()
        {
            if (_mode == Mode.Translate)
                _inputEditTranslate.OnUpdate();

            //if (_mode == Mode.Rotation)
            //    _inputEditTranslate.OnUpdate();
        }

        private bool HasHitPuffObject()
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 100, GeneralFlag.Layer.Anchor);

            return hitCount > 0;
        }

        private void RegisterHeaderBtnEvent() {
            showMoreInfoBtn.onClick.AddListener(() =>
            {
                _mode = Mode.Idle;
            });

            enableTranslationBtn.onClick.AddListener(() =>
            {
                _mode = Mode.Translate;
            });

            enableRotationBtn.onClick.AddListener(() =>
            {
                _mode = Mode.Rotation;
            });



        }

    }
}