using UnityEngine;

namespace StarGarden.UI
{
    public class UIPanel : MonoBehaviour
    {
        protected GameObject UIBase;

        public delegate void UIPanelShowEvent();
        public event UIPanelShowEvent OnShow;
        public delegate void UIPanelHideEvent();
        public event UIPanelHideEvent OnHide;

        public virtual void Initialise() => UIBase = transform.GetChild(0).gameObject;
        public virtual void LateInitialise() { }

        public virtual void Show(object args = null) { UIBase.SetActive(true); OnShow?.Invoke(); }
        public virtual void Hide() { UIBase.SetActive(false); OnHide?.Invoke(); }
    }
}