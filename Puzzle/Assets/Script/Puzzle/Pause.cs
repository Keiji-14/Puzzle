using Scene;
using Audio;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    /// <summary>
    /// ポーズ画面の処理
    /// </summary>
    public class Pause : MonoBehaviour
    {
        #region PublicField
        /// <summary>ポーズ画面を閉じるボタンを押した時の処理</summary>
        public IObservable<Unit> OnClickCloseMenuButtonObserver => closeMenuBtn.OnClickAsObservable();
        #endregion

        #region PrivateField
        /// <summary>リセットボタンを押した時の処理</summary>
        private IObservable<Unit> OnClickResetButtonObserver => resetBtn.OnClickAsObservable();
        /// <summary>タイトル画面の戻るボタンを押した時の処理</summary>
        private IObservable<Unit> OnClickTitleBackButtonObserver => titleBackBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>リセットボタン</summary>
        [SerializeField] private Button resetBtn;
        /// <summary>タイトル画面の戻るボタン</summary>
        [SerializeField] private Button titleBackBtn;
        /// <summary>ポーズ画面を閉じるボタン</summary>
        [SerializeField] private Button closeMenuBtn;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // ゲーム画面をリセットする処理
            OnClickResetButtonObserver.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Puzzle);
            }).AddTo(this);

            // タイトル画面に遷移する処理
            OnClickTitleBackButtonObserver.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Title);
            }).AddTo(this);
        }
        #endregion
    }
}