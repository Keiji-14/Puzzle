using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        #region PrivateField
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

            foreach (var puzzleBoard in puzzleBoardList)
            {
                var isTargetArea = IsTargetArea(puzzlePiece.gameObject.transform.position, puzzleBoard.transform);

                // パズルピースが範囲内に含まれているか
                if (isTargetArea && !puzzleBoard.isSetted)
                {
                    puzzleBoard.isSetted = true;

                    puzzlePiece.SetPuzzle(puzzleBoard.transform);

                    ChackStockPiece(puzzlePiece);

                    return;
                }
            }

            // ストックや盤面の範囲内に含まれていない場合は初期位置に戻す
            puzzlePiece.SetProvPuzzle();
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
        /// 配置したピースがストックからのピースかどうか判定
        /// </summary>
        /// <param name="puzzlePiece">ドロップしたパズルピース</param>
        private void ChackStockPiece(PuzzlePiece puzzlePiece)
        {
            createPuzzlePieceList.Find(piece => piece == puzzlePiece);

            // 生成したパズルピースのリストからかどうかを判定
            if (createPuzzlePieceList.Find(piece => piece == puzzlePiece))
            {
                // ストックの場合はストックを空にする
                createPuzzlePieceList.Remove(puzzlePiece);
            }
            else
            {
                // ストックの場合はストックを空にする
                stockPuzzlePiece = null;
            }

            CheckPuzzleList();

            CheckBoardLine();
        }

        /// <summary>
        /// 盤面に列が出来ているか判定を行う
        /// </summary>
        private void CheckBoardLine()
        {
            for (int i = 0; i < boardMax; i += lineSquareNum)
            {
                List<bool> isHorizontalMatch = new List<bool>();
                
                // 一列毎にマスの状態を取得
                for (int j = i; j < Math.Min(i + lineSquareNum, boardMax); j++)
                {
                    isHorizontalMatch.Add(puzzleBoardList[j].isSetted);
                }

                // 一列出来ているか確認
                if (IsBoardLine(isHorizontalMatch))
                {
                    Debug.Log("Line");
                }
            }
        }

        /// <summary>
        /// 列が出来ているかかうか判定
        /// </summary>
        /// <returns>一列が出来てかどうかの結果</returns>
        private bool IsBoardLine(List<bool> isSquareMatchList)
        {
            var isMatch = true;

            // 列のマス毎に確認する
            foreach (var isSquareMatch in isSquareMatchList)
            {
                // マスにピースが入っているか判定
                if (!isSquareMatch)
                {
                    // 一つでもピースがはまっていない場合はfalseでループ終了
                    isMatch = false;

                    break;
                }
            }

            return isMatch;
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