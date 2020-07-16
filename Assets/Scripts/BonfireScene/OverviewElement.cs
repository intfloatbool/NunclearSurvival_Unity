using UnityEngine;

namespace NunclearGame.BonfireSceneUI
{
    public class OverviewElement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPos;
        public Transform CameraPos => _cameraPos;
        
    }
}
