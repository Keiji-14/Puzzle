using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Puzzle : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 prevPos;

        public void OnBeginDrag(PointerEventData eventData)
        {
            // �h���b�O�O�̈ʒu���L�����Ă���
            prevPos = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // �h���b�O���͈ʒu���X�V����
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}