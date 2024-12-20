using UnityEngine;
using UnityEngine.UI;

public class StartScreen : UIBase
{
   [SerializeField] private Button startButton;

   public override void Show()
   {
      startButton.onClick.AddListener(OnStartButtonClick);
      base.Show();
   }

   public override void Hide()
   {
      startButton.onClick.RemoveListener(OnStartButtonClick);
      base.Hide();
   }

   private void OnStartButtonClick()
   {
      GameManager.Instance.StartGame();
   }
}
