using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>ストックしているパズルピース</summary>
        private PuzzlePiece stockPuzzlePiece;
        /// <summary>生成したパズルのリスト</summary>
        private List<PuzzlePiece> createPuzzlePieceList = new List<PuzzlePiece>();
        #endregion

        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] Transform puzzlePieceParent;
        /// <summary>ストックする場所/summary>
        [SerializeField] Transform stockPos;
        [Header("List")]
        [SerializeField] List<Transform> targetLocationList = new List<Transform>();
        /// <summary>パズルの生成座標</summary>
        [SerializeField] List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>生成するパズルピースのプレハブ</summary>
        [SerializeField] PuzzlePiece puzzlePiece;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            CreatePuzzle();
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

            foreach (var targetLocation in targetLocationList)
            {
                var isTargetArea = IsTargetArea(puzzlePiece.gameObject.transform.position, targetLocation);

                // パズルピースが範囲内に含まれているか
                if (isTargetArea)
                {
                    puzzlePiece.SetPuzzle(targetLocation);

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