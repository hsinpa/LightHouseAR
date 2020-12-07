using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Hsinpa.View
{
    public class DialogueModal : Modal
    {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Text contentText;

        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject buttonPrefab;

        public void SetDialogue(string title, string content, string[] allowBtns, System.Action<int> btnEvent) {
            ResetContent();

            titleText.text = title;
            contentText.text = content;

            RegisterButtons(allowBtns, btnEvent);
        }


        private void RegisterButtons(string[] allowBtns, System.Action<int> btnEvent) {
            int btnlength = allowBtns.Length;

            for (int i = 0; i < btnlength; i++) {
                int index = i;
                GameObject buttonObj = UtilityMethod.CreateObjectToParent(buttonContainer, buttonPrefab);
                Button button = buttonObj.GetComponent<Button>();
                Text textObj = button.GetComponentInChildren<Text>();

                textObj.text = allowBtns[i];

                button.onClick.AddListener(() =>
                {
                    Modals.instance.Close();

                    if (btnEvent != null)
                        btnEvent(index);
                });
            }
        }

        private void ResetContent() {
            UtilityMethod.ClearChildObject(buttonContainer);
        }


    }
}