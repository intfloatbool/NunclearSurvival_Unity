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

            var playerName = GlobalPlayer.Instance.PlayerNickName;
            if (string.IsNullOrEmpty(playerName) || _isAlwaysStart)
            {
                ShowDialogByIndex(0);
            }
        }
    }
}