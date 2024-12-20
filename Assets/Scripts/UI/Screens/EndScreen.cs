using UnityEngine;
using UnityEngine.UI;

public class EndScreen : UIBase
{
   [SerializeField] private Button restartButton;
   [SerializeField] private Button quitButton;

   private void AddListeners()
   {
      quitButton.onClick.AddListener(GameManager.Instance.QuitGame);
   }
   
   private void RemoveListeners()
   {
      quitButton.onClick.RemoveListener(GameManager.Instance.QuitGame);
   }

   public override void Show()
   {
      AddListeners();
      base.Show();
   }

   public override void Hide()
   {
      RemoveListeners();
      base.Hide();
   }
}
