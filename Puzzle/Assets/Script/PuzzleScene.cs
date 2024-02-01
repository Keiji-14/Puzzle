using Scene;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleScene : SceneBase
    {
        #region SerializeField 
        [SerializeField] private PuzzleController puzzleController;
        #endregion

        private new void Start()
        {
            base.Start();

            //puzzleController.Init();    
        }
    }
}