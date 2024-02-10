using Scene;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// タイトル画面の処理
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        #region PrivateField
        /// <summary>元の座標</summary>
        private IObservable<Unit> OnClickGameStartButtonObserver => startBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>ゲーム開始ボタン</summary>
        [SerializeField] Button startBtn;
        #endregion

        #region PublicMethod
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // ゲーム画面に遷移する処理
            OnClickGameStartButtonObserver.Subscribe(_ =>
            {
                SceneLoader.Instance().Load(SceneLoader.SceneName.Puzzle);
            }).AddTo(this);
        }
        #endregion
    }
}