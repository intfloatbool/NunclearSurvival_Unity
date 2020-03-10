using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class GameButtonBase : MonoBehaviour
{
    private Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
