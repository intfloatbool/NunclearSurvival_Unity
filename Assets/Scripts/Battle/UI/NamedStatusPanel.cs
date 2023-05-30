using Common.Values;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.UI
{
    public class NamedStatusPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleTextMesh;
        [SerializeField] private ImageValueWorker _imageValueWorker;


        private void Awake()
        {
            Assert.IsNotNull(_titleTextMesh, "_titleTextMesh != null");
            Assert.IsNotNull(_imageValueWorker, "_imageValueWorker != null");
        }


        public void InitPanel(string titleText, INormalizedValueProvider valueProvider)
        {
            if (_imageValueWorker != null)
            {
                _imageValueWorker.SetValue(valueProvider);
            }
            
            if (_titleTextMesh != null)
            {
                _titleTextMesh.text = titleText;
            }
        }
    }
}
