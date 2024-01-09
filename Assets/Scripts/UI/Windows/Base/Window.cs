using UnityEngine;

namespace UI.Windows.Base
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private WindowType windowType;

        public WindowType WindowType => windowType;

        public void Open()
        {
            gameObject.SetActive(true);
            OnOpen();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            OnClose();
        }

        public virtual void OnOpen() {}
        public virtual void OnClose() {}
    }
}