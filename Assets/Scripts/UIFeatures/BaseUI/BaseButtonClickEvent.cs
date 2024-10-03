using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BaseButtonClickEvent : MonoBehaviour
{
    [SerializeField] private string sound = "button_sound";
    
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(PlaySound);
    }

    void PlaySound()
    {
        MasterAudio.Instance.PlaySound(sound);
    }
}
