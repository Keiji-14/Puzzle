using Scene;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleScene : SceneBase
    {
        #region SerializeField 
        [SerializeField] private PuzzleController puzzleController;
        #endregion

        public override void Start()
        {
            base.Start();

            puzzleController.Init();    
        }
    }
}