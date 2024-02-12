using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CreatePuzzlePiece : EditorWindow
{
    private enum Square
    {
        One,
        Two,
        Three,
        Four,
    }

    #region PrivateField
    /// <summary>配置したパズルピース</summary>
    private string prefabFolderPath = "Assets/Resources/Prefab/Puzzle"; // プレハブのあるフォルダのパス

    private Square horizontal = Square.One;
    private Square vertical = Square.One;

    private List<List<bool>> checkBoxList;
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
                row.Add(false); // チェックボックスの初期値はfalse
            }

            checkBoxList.Add(row);
        }
    }

    /// <summary>
    /// パズルピースのプレハブを生成する処理
    /// </summary>
    private void CreatePuzzlePiecePrefabObject()
    {
        var puzzlePiece = new GameObject("PuzzlePiece");

        // プレハブとして保存
        var prefabPath = prefabFolderPath + "/PuzzlePiece7.prefab";
        PrefabUtility.SaveAsPrefabAsset(puzzlePiece, prefabPath);

        // シーンに追加したオブジェクトは削除
        Destroy(puzzlePiece);

        // リフレッシュして変更を反映
        AssetDatabase.Refresh();
        Debug.Log("Puzzle Pieces Created!");
    }
    #endregion
}