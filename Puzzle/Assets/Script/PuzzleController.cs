﻿using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>列を消去時の値</summary>
        private int destroyLineNum;
        /// <summary>一列のマスの数</summary>
        private const int lineSquareNum = 10;
        /// <summary>盤面のマスの最大値</summary>
        private const int boardMax = 100;
        /// <summary>ストックしているパズルピース</summary>
        private PuzzlePiece stockPuzzlePiece;
        /// <summary>生成したパズルのリスト</summary>
        private List<PuzzlePiece> createPuzzlePieceList = new List<PuzzlePiece>();
        /// <summary>盤面のリスト</summary>
        private List<PuzzleBoard> puzzleBoardList = new List<PuzzleBoard>();
        #endregion

        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] Transform puzzlePieceParent;
        /// <summary>盤面の生成場所</summary>
        [SerializeField] Transform puzzleBoardParent;
        /// <summary>配置したパズルの場所</summary>
        [SerializeField] Transform setPuzzlePieceParent;
        /// <summary>ストックする場所/summary>
        [SerializeField] Transform stockPos;
        /// <summary>パズルの生成座標</summary>
        [SerializeField] List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>生成するパズルピースのプレハブ</summary>
        [SerializeField] PuzzlePiece puzzlePiece;
        /// <summary>生成する配置枠のプレハブ</summary>
        [SerializeField] PuzzleBoard puzzleBoard;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            CreatePuzzle();

            CreateBoard();
        }
        #endregion

        #region PrivateMethod
        /// <summary>
        /// パズルを生成する処理
        /// </summary>
        private void CreatePuzzle()
        {
            // パズルピースを生成する処理
            foreach (var createPos in createPosList)
            {
                var puzzlePieceObj = Instantiate(puzzlePiece, puzzlePieceParent).GetComponent<PuzzlePiece>();

                // 指定した座標に移動
                puzzlePieceObj.transform.localPosition = createPos;

                puzzlePieceObj.Init();

                puzzlePieceObj.DropPieceSubject.Subscribe(puzzle =>
                {
                    DropPiece(puzzle);
                }).AddTo(this);

                createPuzzlePieceList.Add(puzzlePieceObj);
            }
        }

        /// <summary>
        /// 盤面を生成する処理
        /// </summary>
        private void CreateBoard()
        {
            // 盤面を生成する処理
            for (int i = 0; i < boardMax; i++)
            {
                var puzzleBoardObj = Instantiate(puzzleBoard, puzzleBoardParent).GetComponent<PuzzleBoard>();

                puzzleBoardList.Add(puzzleBoardObj);
            }
        }

        /// <summary>
        /// パズルピースをドロップした時の処理
        /// </summary>
        /// <param name="puzzlePiece">ドロップしたパズルピース</param>
        private void DropPiece(PuzzlePiece puzzlePiece)
        {
            SetStockPiece(puzzlePiece);

            ChackPuzzlePiece(puzzlePiece);
        }

        /// <summary>
        /// ピースが範囲内かどうか判定
        /// </summary>
        /// <returns>範囲内かどうかの結果</returns>
        private bool IsTargetArea(Vector3 piecePosition, Transform targetLocation)
        {
            var targetCollider = targetLocation.GetComponent<Collider2D>();

            return targetCollider.OverlapPoint(piecePosition);
        }

        /// <summary>
        /// パズルピースをストックする時の処理
        /// </summary>
        /// <param name="puzzlePiece">ドロップしたパズルピース</param>
        private void SetStockPiece(PuzzlePiece puzzlePiece)
        {
            var isStockArea = IsTargetArea(puzzlePiece.gameObject.transform.position, stockPos);

            // パズルピースがストック範囲内に含まれているか
            // またはストック済みかどうか
            if (isStockArea && stockPuzzlePiece == null)
            {
                stockPuzzlePiece = puzzlePiece;

                puzzlePiece.SetStock(stockPos);

                createPuzzlePieceList.Remove(puzzlePiece);

                CheckPuzzleList();

                return;
            }
        }

        /// <summary>
        /// パズルピースが配置出来るかを確認する処理
        /// </summary>
        /// <param name="puzzlePiece">ドロップしたパズルピース</param>
        private void ChackPuzzlePiece(PuzzlePiece puzzlePiece)
        {
            // foreachで盤面を一時取得する
            List<PuzzleBoard> anSetBoardList = new List<PuzzleBoard>();

            foreach (var piece in puzzlePiece.pieceList)
            {
                foreach (var puzzleBoard in puzzleBoardList)
                {
                    var isSetBoardArea = IsTargetArea(piece.transform.position, puzzleBoard.transform);

                    // パズルピースが範囲内に含まれているか
                    if (isSetBoardArea && puzzleBoard.setPieceObj == null)
                    {
                        piece.isSetted = true;

                        // 配置予定の盤面を追加
                        anSetBoardList.Add(puzzleBoard);

                        break;
                    }
                }
            }

            // 配置出来るかを判定する処理
            var isPlaceable = true;

            foreach (var piece in puzzlePiece.pieceList)
            {
                // 一つでも配置出来ない場合はfalseを返す
                if (!piece.isSetted)
                {
                    isPlaceable = false;
                }
            }

            if (isPlaceable)
            {
                var count = Mathf.Min(puzzlePiece.pieceList.Count, anSetBoardList.Count);

                for (int i = 0; i < count; i++)
                {
                    var piece = puzzlePiece.pieceList[i];
                    var board = anSetBoardList[i];

                    piece.transform.SetParent(setPuzzlePieceParent);
                    piece.transform.localPosition = board.transform.localPosition;

                    board.setPieceObj = piece.gameObject;
                }

                ChackStockPiece(puzzlePiece);

                return;
            }

            // ストックや盤面の範囲内に含まれていない場合は初期位置に戻す
            puzzlePiece.SetProvPuzzle();
        }

        /// <summary>
        /// 配置したピースがストックからのピースかどうか判定
        /// </summary>
        /// <param name="puzzlePiece">ドロップしたパズルピース</param>
        private void ChackStockPiece(PuzzlePiece puzzlePiece)
        {
            createPuzzlePieceList.Find(piece => piece == puzzlePiece);

            // 生成したパズルピースのリストからかどうかを判定
            if (createPuzzlePieceList.Find(piece => piece == puzzlePiece))
            {
                createPuzzlePieceList.Remove(puzzlePiece);
            }
            else
            {
                // ストックの場合はストックを空にする
                stockPuzzlePiece = null;
            }

            CheckBoardLine();

            CheckPuzzleList();
        }

        /// <summary>
        /// 盤面に列が出来ているか判定を行う
        /// </summary>
        private void CheckBoardLine()
        {
            List<PuzzleBoard> destroyPuzzleLineList = new List<PuzzleBoard>();

            // 横の列の判定
            for (int i = 0; i < boardMax; i += lineSquareNum)
            {
                List<PuzzleBoard> horizontalBoardLine = new List<PuzzleBoard>();
                
                // 一列毎にマスを取得
                for (int j = i; j < Math.Min(i + lineSquareNum, boardMax); j++)
                {
                    horizontalBoardLine.Add(puzzleBoardList[j]);
                }

                // 一列出来ているか確認
                if (IsBoardLine(horizontalBoardLine))
                {
                    destroyLineNum++;
                    destroyPuzzleLineList.AddRange(horizontalBoardLine);
                }
            }

            // 縦の列の判定
            for (int i = 0; i < lineSquareNum; i++)
            {
                List<PuzzleBoard> verticalBoardLine = new List<PuzzleBoard>();

                // 一列毎にマスを取得
                for (int j = i; j < boardMax; j += lineSquareNum)
                {
                    Debug.Log(j);
                    verticalBoardLine.Add(puzzleBoardList[j]);
                }

                // 一列出来ているか確認
                if (IsBoardLine(verticalBoardLine))
                {
                    destroyLineNum++;
                    destroyPuzzleLineList.AddRange(verticalBoardLine);
                }
            }

            if (destroyPuzzleLineList.Count != 0)
            {
                DestroyLine(destroyPuzzleLineList);
            }
        }

        /// <summary>
        /// 列が出来ているかかうか判定
        /// </summary>
        /// <returns>一列が出来てかどうかの結果</returns>
        private bool IsBoardLine(List<PuzzleBoard> puzzleBoardList)
        {
            // 列のマス毎に確認する
            foreach (var puzzleBoard in puzzleBoardList)
            {
                // マスにピースが入っているか判定
                if (puzzleBoard.setPieceObj == null)
                {
                    // 一つでもピースがはまっていない場合はfalseを返す
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 完成した列を削除する
        /// </summary>
        private void DestroyLine(List<PuzzleBoard> puzzleBoardList)
        {
            foreach (var puzzleBoard in puzzleBoardList)
            {
                Destroy(puzzleBoard.setPieceObj);

                puzzleBoard.setPieceObj = null;
            }
        }

        /// <summary>
        /// 操作するパズルが残っているか判定を行う
        /// </summary>
        private void CheckPuzzleList()
        {
            if (createPuzzlePieceList != null && createPuzzlePieceList.Count <= 0)
            {
                CreatePuzzle();
            }
        }
        #endregion
    }
}