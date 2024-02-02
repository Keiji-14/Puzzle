﻿using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    public class Puzzle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region PublicField
        /// <summary>ピースをドロップした時の処理</summary>
        public Subject<Puzzle> DropPieceSubject = new Subject<Puzzle>();
        #endregion

        #region PrivateField
        private bool isSetted;
        /// <summary>元の座標</summary>
        private Vector2 prevPos;
        /// <summary>ドラッグ前のサイズ</summary>
        private Vector3 unDraggedSize;
        /// <summary>元のサイズ</summary>
        private Vector3 defaultSize;
        #endregion

        #region EventSystemMethod
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isSetted)
            {
                transform.localScale = defaultSize;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isSetted)
            {
                DropPieceSubject.OnNext(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isSetted)
            {
                // ドラッグ中は位置を更新する
                transform.position = eventData.position;
            }
        }
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // ドラッグ前の位置とサイズを保持する
            prevPos = transform.localPosition;
            defaultSize = transform.localScale;

            // サイズを小さくする
            transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
            // ドラッグ前のサイズとして保持
            unDraggedSize = transform.localScale;
        }

        /// <summary>
        /// パズルを配置する処理
        /// </summary>
        public void SetPuzzle(Transform targetPos)
        {
            // Todo: 配置時の処理を追加する
            transform.localPosition = targetPos.localPosition;
            isSetted = true;
        }

        /// <summary>
        /// パズルを元の場所に設定する処理
        /// </summary>
        public void SetProvPuzzle()
        {
            transform.localPosition = prevPos;
            transform.localScale = unDraggedSize;
        }
        #endregion
    }
}