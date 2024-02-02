using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Puzzle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region PrivateField
        /// <summary>元の座標</summary>
        private Vector2 prevPos;
        /// <summary>ドラッグ前のサイズ</summary>
        private Vector3 unDraggedSize;
        /// <summary>元のサイズ</summary>
        private Vector3 defaultSize;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // ドラッグ前の位置とサイズを保持する
            prevPos = transform.position;
            defaultSize = transform.localScale;

            // サイズを小さくする
            transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
            // ドラッグ前のサイズとして保持
            unDraggedSize = transform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = defaultSize;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.localScale = unDraggedSize;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // ドラッグ中は位置を更新する
            transform.position = eventData.position;
        }
        #endregion
    }
}