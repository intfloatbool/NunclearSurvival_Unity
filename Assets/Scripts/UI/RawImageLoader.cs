using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageLoader : MonoBehaviour
    {
        [Header("Path inside the Resources folder.")]
        [SerializeField] private string _texturePath;
        
        [Space(3f)]
        [SerializeField] private Color _colorAfterLoad = Color.white;

        private RawImage _rawImage;
        
        private void Start()
        {
            _rawImage = GetComponent<RawImage>();
            LoadImage();
        }
    
        private void LoadImage()
        {
            var texture = Resources.Load<Texture2D>(_texturePath);
            _rawImage.texture = texture;
            _rawImage.color = _colorAfterLoad;
        }
    }

}
