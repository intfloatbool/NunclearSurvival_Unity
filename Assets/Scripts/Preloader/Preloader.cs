using SingletonsPreloaders;
using System.Collections;
using System.Collections.Generic;
using Common.Dependencies;
using UnityEngine;

public class Preloader : MonoBehaviour
{
    [SerializeField] private List<GameObject> _managerPrefabs;
    [SerializeField] private float _delayAfterPreload = 2f;

    private IEnumerator Start()
    {
        yield return StartCoroutine(PreloadManagersCoroutine());
    }

    private IEnumerator PreloadManagersCoroutine()
    {
        foreach(var managerPrefab in _managerPrefabs)
        {
            var manager = Instantiate(managerPrefab);
            var singletonDep = manager.GetComponent<ISingletonDependency>();
            if (singletonDep != null)
            {
                singletonDep.SelfRegister();
            }
            DontDestroyOnLoad(manager);
        }

        yield return new WaitForSeconds(_delayAfterPreload);

        SceneLoader.Instance.LoadScene(SceneType.MAIN_MENU);
    }
}
