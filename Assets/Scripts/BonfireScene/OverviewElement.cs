using UnityEngine;

namespace NunclearGame.BonfireSceneUI
{
    public class OverviewElement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraPos;
        public Transform CameraPos => _cameraPos;
        
        [SerializeField] private GameObject[] _relativeObjects;
        
        public void SetActiveRelativeObjects(bool isActive)
        {
            for (int i = 0; i < _relativeObjects.Length; i++)
            {
                var relativeObject = _relativeObjects[i];
                if (relativeObject != null)
                {
                    relativeObject.SetActive(isActive);
                }
            }
        }
        
    }
}
