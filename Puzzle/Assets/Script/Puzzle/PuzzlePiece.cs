﻿using Audio;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    /// <summary>
    /// パズルピースの操作管理の処理
    /// </summary>
    public class PuzzlePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region PublicField
        /// <summary>ピースをドロップした時の処理</summary>
        public Subject<PuzzlePiece> DropPieceSubject = new Subject<PuzzlePiece>();
        /// <summary>ピースの形状</summary>
        public List<int> pieceSquareList = new List<int>();
        /// <summary>ピースの一マス</summary>
        public List<Piece> pieceList = new List<Piece>();
        #endregion

        #region PrivateField
        /// <summary>サイズを小さくする為の値</summary>
        private const float setUnDraggedSize = 2.5f;
        /// <summary>元の座標</summary>
        private Vector2 prevPos;
        /// <summary>ドラッグ前のサイズ</summary>
        private Vector3 unDraggedSize;
        /// <summary>元のサイズ</summary>
        private Vector3 defaultSize;
        #endregion

        #region SerializeField
        /// <summary>配置場所が無い時のスプライト</summary>
        [SerializeField] private Sprite unSetSprite;
        #endregion

        #region EventSystemMethod
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.localScale = defaultSize;
            SE.instance.Play(SE.SEName.DragSE);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DropPieceSubject.OnNext(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // ドラッグ中は位置を更新する
            transform.position = eventData.position;
        }
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // ピースの形状を保持
            foreach (var piece in pieceList)
            {
                piece.Init();

                pieceSquareList.Add(piece.squareID);
            }

            // ドラッグ前の位置とサイズを保持する
            prevPos = transform.localPosition;
            defaultSize = transform.localScale;

            // サイズを小さくする
            transform.localScale = 
                new Vector3(transform.localScale.x / setUnDraggedSize, transform.localScale.y / setUnDraggedSize, transform.localScale.z / setUnDraggedSize);
            // ドラッグ前のサイズとして保持
            unDraggedSize = transform.localScale;
        }

        /// <summary>
        /// パズル配置後に削除する
        /// </summary>
        public void DestroyPuzzlePiece()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// パズルをストックする処理
        /// </summary>
        /// <param name="targetPos">パズルピースをストックする場所</param>
        public void SetStock(Transform targetPos)
        {
            transform.localPosition = targetPos.localPosition;
            transform.localScale = unDraggedSize;

            // 初期座標をストック場所に変更
            prevPos = transform.localPosition;
        }

        /// <summary>
        /// パズルを元の場所に設定する処理
        /// </summary>
        public void SetProvPuzzle()
        {
            transform.localPosition = prevPos;
            transform.localScale = unDraggedSize;
        
            // 設置状態を解除
            foreach (var piece in pieceList)
            {
                piece.isSetted = false;
            }
        }

        /// <summary>
        /// 通常のスプライトに切り替える
        /// </summary>
        public void SwitchDefaultSprite()
        {
            foreach (var piece in pieceList)
            {
                piece.SetDefaultSprite();
            }
        }

        /// <summary>
        /// 配置場所が無い時のスプライトに切り替える
        /// </summary>
        public void SwitchUnSetSprite()
        {
            foreach (var piece in pieceList)
            {
                piece.SetUnSetSprite(unSetSprite);
            }
        }
        #endregion
    }
}