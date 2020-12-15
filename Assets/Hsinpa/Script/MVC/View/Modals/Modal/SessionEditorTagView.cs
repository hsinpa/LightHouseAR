using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace Hsinpa.View
{
    public class SessionEditorTagView : MonoBehaviour
    {
        [SerializeField]
        private Text session_name;

        [SerializeField]
        private Button deleteBtn;

        public void SetUp(string p_session, System.Action<GameObject> deleteEvent) {
            this.session_name.text = p_session;

            deleteBtn.onClick.AddListener(() =>
            {
                deleteEvent(this.gameObject);
            });
        }

    }
}