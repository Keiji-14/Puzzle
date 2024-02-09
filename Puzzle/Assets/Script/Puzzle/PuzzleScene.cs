using Scene;
using UnityEngine;

namespace Puzzle
{
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
        }
        #endregion
    }
}