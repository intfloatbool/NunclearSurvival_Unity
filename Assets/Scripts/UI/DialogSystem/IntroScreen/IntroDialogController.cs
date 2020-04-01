using Player;
using UnityEngine;

namespace GameUI
{
    public class IntroDialogController : DialogController
    {
        [SerializeField] private bool _isAlwaysStart = false;
        protected override void Start()
        {
            base.Start();
            
            if (!GlobalPlayer.Instance.IsPlayerReady || _isAlwaysStart)
            {
                ShowDialogByIndex(0);
            }
        }
    }
}