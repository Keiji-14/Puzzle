using Scene;
using Audio;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
public class Pause : MonoBehaviour
{
        #region PrivateField
        /// <summary>���Z�b�g�{�^�������������̏���</summary>
        private IObservable<Unit> OnClickResetButtonObserver => resetBtn.OnClickAsObservable();
        /// <summary>�^�C�g����ʂ̖߂�{�^�������������̏���</summary>
        private IObservable<Unit> OnClickTitleBackButtonObserver => titleBackBtn.OnClickAsObservable();
        #endregion

        #region SerializeField
        /// <summary>���Z�b�g�{�^��</summary>
        [SerializeField] Button resetBtn;
        /// <summary>�^�C�g����ʂ̖߂�{�^��</summary>
        [SerializeField] Button titleBackBtn;
        #endregion

        #region PublicMethod
        /// <summary>
        /// ������
        /// </summary>
        public void Init()
        {
            // �Q�[����ʂ����Z�b�g���鏈��
            OnClickResetButtonObserver.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Puzzle);
            }).AddTo(this);

            // �^�C�g����ʂɑJ�ڂ��鏈��
            OnClickTitleBackButtonObserver.Subscribe(_ =>
            {
                SE.instance.Play(SE.SEName.ButtonSE);
                SceneLoader.Instance().Load(SceneLoader.SceneName.Title);
            }).AddTo(this);
        }
        #endregion
    }
}