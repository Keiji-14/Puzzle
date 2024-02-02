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
        #endregion

        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] Transform puzzleParent;
        [Header("List")]
        [SerializeField] List<Transform> targetLocationList = new List<Transform>();
        /// <summary>パズルの生成座標</summary>
        [SerializeField] List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>パズル</summary>
        [SerializeField] Puzzle puzzle;
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
            // パズルを生成する処理
            foreach (var createPos in createPosList)
            {
                var puzzleObj = Instantiate(puzzle, puzzleParent).GetComponent<Puzzle>();

                // 指定した座標に移動
                puzzleObj.transform.localPosition = createPos;

                puzzleObj.Init();

                puzzleObj.DropPieceSubject.Subscribe(puzzle =>
                {
                    DropPiece(puzzle);
                }).AddTo(this);   
            }
        }

        /// <summary>
        /// パズルピースをドロップした時の処理
        /// </summary>
        private void DropPiece(Puzzle puzzle)
        {
            foreach (var targetLocation in targetLocationList)
            {
                isTargetArea = IsTargetArea(puzzle.gameObject.transform.position, targetLocation);

                // パズルが範囲内に含まれているか
                if (isTargetArea)
                {
                    puzzle.SetPuzzle();

                    return;
                }
                
            }

            puzzle.SetProvPuzzle();
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
        #endregion
    }
}