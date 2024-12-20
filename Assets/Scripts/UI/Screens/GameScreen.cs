using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : UIBase
{
     [SerializeField] private Slider healthSlider;
     [SerializeField] private TextMeshProUGUI goldCoinsText;

     public override void Show()
     {
          healthSlider.maxValue = GameManager.Instance.HouseMaxHealth;
          healthSlider.value = healthSlider.maxValue;
          goldCoinsText.text = GameManager.Instance.GetCoins.ToString();
          
          GameEvents.OnHouseHit += GameEventsOnOnHouseHit;
          GameEvents.OnCoinsChanged += UpdateCoinsDisplay;
          
          base.Show();
     }

     private void GameEventsOnOnHouseHit(int damage)
     {
          healthSlider.value -= damage;
     }
     
     private void UpdateCoinsDisplay(int coins)
     {
          goldCoinsText.text = coins.ToString();
     }

     public override void Hide()
     {
          GameEvents.OnHouseHit -= GameEventsOnOnHouseHit;
          GameEvents.OnCoinsChanged -= UpdateCoinsDisplay;
          base.Hide();
     }
}
