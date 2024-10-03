using GameFoundation.Scripts.Utilities.Extension;
using UnityEngine;
using UserData.Controller;
using Zenject;

public class EscButton : MonoBehaviour
{
    [Inject] private LevelManager levelManager;

    public Sprite normal;
    public Sprite pressed;
    
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer        = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normal;
    }
    

    private void Start()
    {
        this.GetCurrentContainer().Inject(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>())
        {
            spriteRenderer.sprite = pressed;
            levelManager.ShowLoseScreen();
        }
    }
    
}
