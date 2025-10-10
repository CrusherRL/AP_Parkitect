using UnityEngine;
using UnityEngine.EventSystems;

namespace ArchipelagoMod.Src.Window.Scripts
{
    class DragUI : MonoBehaviour, IDragHandler
    {
        public Canvas Canvas; // the Canvas
        private RectTransform RectTransform; // the Frame

        void OnEnable()
        {
            this.RectTransform = transform.GetComponent<RectTransform>();
            this.Canvas = transform.parent.GetComponent<Canvas>();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (this.RectTransform == null)
            {
                return;
            }
            if (this.Canvas == null)
            {
                return;
            }

            this.RectTransform.anchoredPosition += eventData.delta / this.Canvas.scaleFactor;
        }
    }
}