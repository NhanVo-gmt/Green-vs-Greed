namespace UIFeatures.GameScene
{
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class QuizButton : MonoBehaviour
    {
        public Button          button;
        public Image           image;
        public TextMeshProUGUI text;

        public string answer;
        
        public  Action<string> OnClick;

        public void BindData(int index, string answer)
        {
            this.answer = answer;
            this.text.SetText($"{index}. {answer}");
            button.onClick.AddListener(() => OnClick?.Invoke(answer));
        }

        public void Dispose()
        {
            button.onClick.RemoveAllListeners();
            image.color = Color.white;
        }

        public void UpdateAnswerColor(Color color)
        {
            image.color = color;
        }
    }
}