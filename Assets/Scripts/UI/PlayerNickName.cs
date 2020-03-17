using System;
using Player;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class PlayerNickName : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private string _unknownKey = "?UNKNOWN?";
        private void Start()
        {
            var globalPlayer = GlobalPlayer.Instance;
            if (globalPlayer != null)
            {
                _textMesh.text = !string.IsNullOrEmpty(globalPlayer.PlayerNickName)
                    ? globalPlayer.PlayerNickName
                    : _unknownKey;
                
                globalPlayer.OnPlayerNameUpdated += (freshName) => { _textMesh.text = freshName; };
            }
            else
            {
                _textMesh.text = _unknownKey;
            }
            
            
        }
    }

}
