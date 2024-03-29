﻿using Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Puzzle
{
    /// <summary>
    /// パズル画面の処理
    /// </summary>
    public class PuzzleController : MonoBehaviour
    {
        #region PublicField
        /// <summary>ゲームオーバー時の処理</summary>
        public Subject<Unit> GameOverSubject = new Subject<Unit>();
        #endregion

        #region PrivateField
        /// <summary>スコアの値</summary>
        private int scoreNum;
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
        /// <summary>ポーズボタンを押した時の処理</summary>
        private IObservable<Unit> OnClickPauseButtonObserver => pauseBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>パズルの生成場所</summary>
        [SerializeField] private Transform puzzlePieceParent;
        /// <summary>盤面の生成場所</summary>
        [SerializeField] private Transform puzzleBoardParent;
        /// <summary>配置したパズルの場所</summary>
        [SerializeField] private Transform setPuzzlePieceParent;
        /// <summary>ストックする場所</summary>
        [SerializeField] private Transform stockPos;
        /// <summary>ラインが消えた時に表示する</summary>
        [SerializeField] private TextMeshProUGUI comboText;
        /// <summary>ポーズ画面のボタン</summary>
        [SerializeField] private Button pauseBtn;
        /// <summary>パズルの生成座標</summary>
        [SerializeField] private List<Vector3> createPosList = new List<Vector3>();
        [Header("Component")]
        /// <summary>生成する配置枠のプレハブ</summary>
        [SerializeField] private PuzzleBoard puzzleBoard;
        /// <summary>スコア</summary>
        [SerializeField] private Score score;
        /// <summary>ポーズ画面</summary>
        [SerializeField] private Pause pause;
        /// <summary>パズルピースをストックするように促すUI</summary>
        [SerializeField] private BlinkUI settingStockUI;
        /// <summary>ストックからパズルピースを外すように促すUI</summary>
        [SerializeField] private BlinkUI unfastenStockUI;
        /// <summary>生成するパズルピースのプレハブ</summary>
        [SerializeField] private List<PuzzlePiece> puzzlePieceList;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            CreateBoard();

            CreatePuzzle();

            score.Init();

            PauseInit();
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
                var randIndex = UnityEngine.Random.Range(0, puzzlePieceList.Count);
                var puzzlePieceObj = Instantiate(puzzlePieceList[randIndex], puzzlePieceParent).GetComponent<PuzzlePiece>();

                // 指定した座標に移動
                puzzlePieceObj.transform.localPosition = createPos;

                puzzlePieceObj.Init();

                puzzlePieceObj.DropPieceSubject.Subscribe(puzzle =>
                {
                    DropPiece(puzzle);
                }).AddTo(this);

                createPuzzlePieceList.Add(puzzlePieceObj);

                ChackPuzzleState(puzzlePieceObj);
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
        /// ポーズ関係の初期化
        /// </summary>
        private void PauseInit()
        {
            pause.Init();

            pause.gameObject.SetActive(false);

            OnClickPauseButtonObserver.Subscribe(_ =>
            {
                SwitchPauseMenu();
            }).AddTo(this);

            pause.OnClickCloseMenuButtonObserver.Subscribe(_ =>
            {
                SwitchPauseMenu();
            }).AddTo(this);
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

                settingStockUI.gameObject.SetActive(false);

                unfastenStockUI.gameObject.SetActive(false);

                CheckPuzzleList();

                CheckGameOver();

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

                    scoreNum++;
                }

                ChackStockPiece(puzzlePiece);

                // 表示しているスコアを更新
                score.UpdataScore(scoreNum);

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

            SE.instance.Play(SE.SEName.SetSE);

            puzzlePiece.DestroyPuzzlePiece();

            CheckBoardLine();

            CheckPuzzleList();

            CheckGameOver();
        }

        /// <summary>
        /// 盤面に列が出来ているか判定を行う
        /// </summary>
        private void CheckBoardLine()
        {
            List<PuzzleBoard> destroyPuzzleLineList = new List<PuzzleBoard>();

            // 消したライン数
            var destroyLineNum = 0;

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
                    verticalBoardLine.Add(puzzleBoardList[j]);
                }

                // 一列出来ているか確認
                if (IsBoardLine(verticalBoardLine))
                {
                    destroyLineNum++;
                    destroyPuzzleLineList.AddRange(verticalBoardLine);
                }
            }

            // 列が出来ているかどうか
            if (destroyPuzzleLineList.Count != 0)
            {
                DestroyLine(destroyPuzzleLineList);

                AddScore(destroyLineNum);

                StartCoroutine(ViewComboText(destroyLineNum));
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
        /// スコアを加算する処理
        /// </summary>
        /// <param name="destroyLineNum">消去したライン数</param>
        private void AddScore(int destroyLineNum)
        {
            for (int i = 0; i < destroyLineNum; i++)
            {
                var getScoreNum = 10 + i * 5;

                // スコアに加算
                scoreNum += getScoreNum;
            }
        }

        /// <summary>
        /// 列を消したときの演出
        /// </summary>
        /// <param name="destroyLineNum">消去したライン数</param>
        private IEnumerator ViewComboText(int destroyLineNum)
        {
            var waitTime = 1.0f;

            comboText.gameObject.SetActive(true);

            comboText.text = $"{destroyLineNum}Combo";

            yield return new WaitForSeconds(waitTime);

            comboText.gameObject.SetActive(false);
        }

        /// <summary>
        /// 完成した列を削除する
        /// </summary>
        /// <param name="puzzleBoardList">列が出来た盤面</param>
        private void DestroyLine(List<PuzzleBoard> puzzleBoardList)
        {
            SE.instance.Play(SE.SEName.DestroySE);

            foreach (var puzzleBoard in puzzleBoardList)
            {
                Destroy(puzzleBoard.setPieceObj);

                puzzleBoard.setPieceObj = null;
            }
        }

        /// <summary>
        /// 操作するパズルピースの状態を確認する処理
        /// </summary>
        private void CheckPuzzleList()
        {
            foreach (var puzzlePiece in createPuzzlePieceList)
            {
                ChackPuzzleState(puzzlePiece);
            }

            // ストックのパズルピースの状態を確認する
            ChackPuzzleState(stockPuzzlePiece);

            if (createPuzzlePieceList != null && createPuzzlePieceList.Count <= 0)
            {
                CreatePuzzle();
            }
        }

        /// <summary>
        /// 操作するパズルピースの状態を確認する処理
        /// </summary>
        private void ChackPuzzleState(PuzzlePiece puzzlePiece)
        {
            if (puzzlePiece == null)
            {
                return;
            }

            for (int i = 0; i < puzzleBoardList.Count; i++)
            {
                // はめ込める場所があるなら通常スプライト
                if (IsCanSetPuzzlePiece(i, puzzlePiece.pieceSquareList))
                {
                    puzzlePiece.SwitchDefaultSprite();
                    break;
                }
                else
                {
                    puzzlePiece.SwitchUnSetSprite();
                }
            }
        }

        /// <summary>
        /// ゲームオーバーかどうか確認する
        /// </summary>
        private void CheckGameOver()
        {
            if (IsGameOver())
            {
                // スコアを記録する
                SaveScore();

                GameOverSubject.OnNext(Unit.Default);
            }
        }

        /// <summary>
        /// ゲームオーバー判定
        /// </summary>
        private bool IsGameOver()
        {
            foreach (var puzzlePiece in createPuzzlePieceList)
            {
                for (int i = 0; i < puzzleBoardList.Count; i++)
                {
                    if (IsCanSetPuzzlePiece(i, puzzlePiece.pieceSquareList))
                    {
                        return false;
                    }
                }
            }

            // ストックの枠が空いている場合
            if (stockPuzzlePiece == null)
            {
                // パズルピースをストックさせるように促す
                settingStockUI.gameObject.SetActive(true);
                settingStockUI.ShowUI();

                return false;
            }
            else
            {
                // ストック枠のパズルピースがハマる場所がある場合
                for (int i = 0; i < puzzleBoardList.Count; i++)
                {
                    if (IsCanSetPuzzlePiece(i, stockPuzzlePiece.pieceSquareList))
                    {
                        // ストックからパズルピースを外すように促す
                        unfastenStockUI.gameObject.SetActive(true);
                        unfastenStockUI.ShowUI();

                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 特定のピースのはめ込む場所があるかの判定
        /// </summary>
        /// <param name="startIndex">形状の判定を開始する位置のインデックス</param>
        /// <param name="pieceSquareList">ピースの形状</param>
        private bool IsCanSetPuzzlePiece(int startIndex, List<int> pieceSquareList)
        {
            foreach (int offset in pieceSquareList)
            {
                int currentIndex = startIndex + offset;

                // 範囲外の場合やピースが配置されている場合、指定した形状がはめ込めない
                if (currentIndex >= puzzleBoardList.Count ||
                    currentIndex % lineSquareNum < startIndex % lineSquareNum ||
                    puzzleBoardList[currentIndex].setPieceObj != null)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// 記録がハイスコアか確認する処理
        /// </summary>
        private void SaveScore()
        {
            var highScoreNum = PlayerPrefs.GetInt("HighScore", 0);

            // 獲得したスコアがハイスコアを越しているかの判定
            if (scoreNum > highScoreNum)
            {
                PlayerPrefs.SetInt("HighScore", scoreNum);
            }
        }

        /// <summary>
        /// ポーズ画面の表示切替の処理
        /// </summary>
        private void SwitchPauseMenu()
        {
            SE.instance.Play(SE.SEName.ButtonSE);
            pause.gameObject.SetActive(!pause.gameObject.activeSelf ? true : false);
        }
        #endregion
    }
}