using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TestARFoundationInit : MonoBehaviour
{
    [SerializeField]
    private ARSession arSession;

    [SerializeField]
    private Button testARBtn;

    private void Start()
    {
        testARBtn.onClick.AddListener(OnTestARBtnClick);
    }

    private void OnTestARBtnClick() {
        arSession.enabled = !arSession.enabled;
    }
}
