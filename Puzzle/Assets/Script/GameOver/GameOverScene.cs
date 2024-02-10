using Scene;
using UnityEngine;

namespace GameOver
{
    /// <summary>
    /// ゲームオーバー画面の管理
    /// </summary>
    public class GameOverScene : SceneBase
    {
        #region SerializeField 
        /// <summary>ゲームオーバー画面の処理</summary>
        [SerializeField] private GameOverController gameOverController;
        #endregion

        #region UnityEvent
        public override void Start()
        {
            base.Start();

            gameOverController.Init();
        }
        #endregion
    }
}