using Puzzle;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CreatePuzzlePiece : EditorWindow
{
    #region PrivateField
    /// <summary>配置したパズルピース</summary>
    private string prefabFolderPath = "Assets/Resources/Prefab/Puzzle"; // プレハブのあるフォルダのパス

    private Square horizontal = Square.One;
    private Square vertical = Square.One;

    private Sprite selectedSprite;

    private List<List<bool>> checkBoxList;
    #endregion

    #region PublicMethod
    private enum Square
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }
    #endregion

    #region PrivateMethod
    /// <Summary>
    /// ウィンドウを表示
    /// </Summary>
    [MenuItem("Editor/CreatePuzzlePiece")]
    private static void Init()
    {
        // 生成
        GetWindow<CreatePuzzlePiece>("CreatePuzzlePiece");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Folder Path:");
        prefabFolderPath = EditorGUILayout.TextField(prefabFolderPath);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        // horizontalの選択
        GUILayout.Label("Horizontal:");
        horizontal = (Square)EditorGUILayout.Popup((int)horizontal, Enum.GetNames(typeof(Square)));
        // verticalの選択
        GUILayout.Label("Vertical:");
        vertical = (Square)EditorGUILayout.Popup((int)vertical, Enum.GetNames(typeof(Square)));
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label("Select a Sprite:", EditorStyles.boldLabel);

        GUILayout.Space(10);

        // スプライトの選択
        selectedSprite = EditorGUILayout.ObjectField("Sprite", selectedSprite, typeof(Sprite), false) as Sprite;

        // チェックボックスの表示を実行するボタン
        if (GUILayout.Button("Set Square Board"))
        {
            SetSquareBoard();
        }        

        GUILayout.Space(10);

        // チェックボックスの表示
        if (checkBoxList != null)
        {
            for (int x = 0; x < checkBoxList.Count; x++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Space(50); // 左にスペースを挿入

                for (int y = 0; y < checkBoxList[x].Count; y++)
                {
                    checkBoxList[x][y] = EditorGUILayout.Toggle(checkBoxList[x][y]);
                }

                GUILayout.Space(200); // 左にスペースを挿入

                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);

        // パズルピースの生成を実行するボタン
        if (GUILayout.Button("Create Puzzle Piece"))
        {
            CreatePuzzlePiecePrefabObject();
        }
    }

    private void SetSquareBoard()
    {
        // チェックボックスを管理するListを初期化
        checkBoxList = new List<List<bool>>();

        for (int x = 0; x < (int)horizontal + 1; x++)
        {
            List<bool> row = new List<bool>();

            for (int y = 0; y < (int)vertical+ 1; y++)
            {
                // チェックボックスの初期値はfalse
                row.Add(false);
            }

            checkBoxList.Add(row);
        }
    }

    /// <summary>
    /// パズルピースのプレハブを生成する処理
    /// </summary>
    private void CreatePuzzlePiecePrefabObject()
    {
        // スプライトがnullの場合は生成出来ない
        if (selectedSprite == null)
        {
            Debug.LogError("Sprite is null.");
            return;
        }

        // パズルピースの操作部分を生成する
        var puzzlePiece = new GameObject("PuzzlePiece", typeof(RectTransform), typeof(PuzzlePiece)).GetComponent<PuzzlePiece>();

        int rowCount = checkBoxList.Count;
        int colCount = rowCount > 0 ? checkBoxList[0].Count : 0;

        // チェックボックスの状態を取得
        for (int x = 0; x < rowCount; x++) 
        {
            for (int y = 0; y < colCount; y++)
            {
                if (checkBoxList[x][y])
                {
                    // ピース部分を生成する
                    var piece = new GameObject("Piece", typeof(Image), typeof(Piece)).GetComponent<Piece>();
                    piece.transform.SetParent(puzzlePiece.transform);

                    SetPiece(piece, x, y);

                    puzzlePiece.pieceList.Add(piece);
                }
            }
        }

        // フォルダ内のプレハブファイルを取得
        string[] prefabPaths = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });

        Debug.Log(prefabPaths.Length);
        // プレハブファイル数に応じて名番号を設定・保存
        var prefabPath = prefabFolderPath + $"/PuzzlePiece{prefabPaths.Length + 1}.prefab";
        PrefabUtility.SaveAsPrefabAsset(puzzlePiece.gameObject, prefabPath);

        // リフレッシュして変更を反映
        AssetDatabase.Refresh();
        Debug.Log("Puzzle Pieces Created!");
    }

    /// <summary>
    /// ピースの状態を設定する処理
    /// </summary>
    /// <param name="piece">パズルピースの1マス</param>
    /// <param name="x">X座標</param>
    /// <param name="y">Y座標</param>
    private void SetPiece(Piece piece, int x, int y)
    {
        // マスの番号を設定
        piece.squareID = x * 10 + y;

        // ピースのサイズ
        var pieceSize = 90f;
        // ピースの間隔
        var spacing = 10f;

        var pieceRect = piece.GetComponent<RectTransform>();
        var pieceImg = piece.GetComponent<Image>();

        var offsetX = (checkBoxList.Count - 1) * (pieceSize + spacing) / 2;
        var offsetY = (checkBoxList[x].Count - 1) * (pieceSize + spacing) / 2;

        // ピースの座標を設定する
        var pieceX = y * (pieceSize + spacing) - offsetY;
        var pieceY = -x * (pieceSize + spacing) + offsetX;
        
        piece.transform.localPosition = new Vector3(pieceX, pieceY, 0);

        pieceRect.sizeDelta = new Vector2(pieceSize, pieceSize);
        pieceImg.sprite = selectedSprite;
    }
    #endregion
}