using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>範囲内に含まれているか</summary>
        private bool isTargetArea;
        private List<PuzzlePiece> puzzlePieceList = new List<PuzzlePiece>();
        #endregion

        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] Transform puzzlePieceParent;
        [Header("List")]
        [SerializeField] List<Transform> targetLocationList = new List<Transform>();
        /// <summary>パズルの生成座標</summary>
        [SerializeField] List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>パズル</summary>
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

                puzzlePieceList.Add(puzzlePieceObj);
            }
        }

        /// <summary>
        /// パズルピースをドロップした時の処理
        /// </summary>
        private void DropPiece(PuzzlePiece puzzlePiece)
        {
            foreach (var targetLocation in targetLocationList)
            {
                isTargetArea = IsTargetArea(puzzlePiece.gameObject.transform.position, targetLocation);

                // パズルが範囲内に含まれているか
                if (isTargetArea)
                {
                    puzzlePiece.SetPuzzle(targetLocation);

                    puzzlePieceList.Remove(puzzlePiece);

                    CheckPuzzleList();

                    return;
                }
                
            }

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
        /// 操作するパズルが残っているか判定を行う
        /// </summary>
        private void CheckPuzzleList()
        {
            if (puzzlePieceList != null && puzzlePieceList.Count <= 0)
            {
                CreatePuzzle();
            }
        }
        #endregion
    }
}