using UnityEngine.UI;
using UnityEngine;
using System.Collections;

namespace SingletonsPreloaders
{
    public class LoadingUI : UnitySingletonBase<LoadingUI>
    {
        [SerializeField] private Transform _loadingTransform;
        [SerializeField] private float _loadingSpeed = 5f;
        [SerializeField] private float _delayAfterLoading = 1f;
        [SerializeField] private Image _loadingBar;
        [SerializeField] private bool _isSceneLoading;
        [SerializeField] private float _currentProgress = 0.5f;
        [SerializeField] private float _basicProgress = 0.3f;
        private Coroutine _finishingCoroutine;

        private void Start()
        {
            SceneLoader.Instance.OnSceneBeginDownloading += OnSceneBeginLoading;
            SceneLoader.Instance.OnSceneFinishDownloading += OnSceneFinishDownloading;
        }

        private void OnSceneBeginLoading(SceneType sceneType)
        {
            _loadingBar.fillAmount = 0f;
            _isSceneLoading = true;
            _currentProgress = _basicProgress;
            SetActiveTransforom(true);
        }

        private void OnSceneFinishDownloading(SceneType sceneType)
        {
            _isSceneLoading = false;
            if (_finishingCoroutine != null)
            {
                StopCoroutine(_finishingCoroutine);
            }

            var loaderDelay = SceneLoader.Instance.CurrentDelay;
            if (loaderDelay != null)
            {
                _finishingCoroutine = StartCoroutine(FinishingCoroutine(loaderDelay.Value));
            }
            else
            {
                _finishingCoroutine = StartCoroutine(FinishingCoroutine(_delayAfterLoading));
            }

        }

        private void SetActiveTransforom(bool isActive)
        {
            _loadingTransform.gameObject.SetActive(isActive);
            _loadingBar.fillAmount = 0f;
        }

        private IEnumerator FinishingCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetActiveTransforom(false);
            _finishingCoroutine = null;
        }

        private void Update()
        {
            if (!_isSceneLoading)
            {
                _currentProgress = 0f;
                return;
            }

            _currentProgress = SceneLoader.Instance.CurrentProgress;
            _loadingBar.fillAmount = Mathf.Clamp(Mathf.Lerp(_loadingBar.fillAmount, _currentProgress, _loadingSpeed * Time.deltaTime), 0, 1f);
        }

        protected override LoadingUI GetInstance() => this;
    }
}
