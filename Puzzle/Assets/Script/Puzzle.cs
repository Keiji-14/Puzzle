using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Puzzle : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 prevPos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // ドラッグ前の位置を記憶しておく
            prevPos = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // ドラッグ中は位置を更新する
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}