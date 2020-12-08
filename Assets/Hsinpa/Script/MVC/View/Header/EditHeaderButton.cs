using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditHeaderButton : MonoBehaviour
{
    [SerializeField]
    private Image buttonHint;

    [SerializeField]
    private Button _Button;
    public Button Button => _Button;

    public void ActivateHint(bool isActivate) {
        buttonHint.gameObject.SetActive(isActivate);
    }
}
