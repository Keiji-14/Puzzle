using Scene;
using UniRx;
using UnityEngine;

namespace Puzzle
{
    /// <summary>
    /// パズル画面の管理
    /// </summary>
    public class PuzzleScene : SceneBase
    {
        #region SerializeField 
        [SerializeField] private PuzzleController puzzleController;
        #endregion

        #region UnityEvent
        public override void Start()
        {
            base.Start();

            puzzleController.Init();

            puzzleController.GameOverSubject.Subscribe(_ =>
            {
                SceneLoader.Instance().Load(SceneLoader.SceneName.GameOver, true);
            }).AddTo(this);

        }
        #endregion
    }
}