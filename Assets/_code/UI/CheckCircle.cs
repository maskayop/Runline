using UnityEngine;

namespace Runline
{
    public abstract class CheckCircle : MonoBehaviour
    {
        public bool isSelected = false;
        public int id = -1;

        [SerializeField] GameObject unselected;
        [SerializeField] GameObject selected;

        public virtual void Init()
        {
            OnInit();
        }

        protected virtual void OnInit() { }

        public void Select(bool state)
        {
            isSelected = state;
            selected.SetActive(isSelected);
            unselected.SetActive(!isSelected);

            OnSelected();
        }

        protected virtual void OnSelected() { }

        public virtual void OnClick() { }
    }
}
