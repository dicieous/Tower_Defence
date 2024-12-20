using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct WeaponUI
{
    public WeaponList weapon;
    public Button button;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI priceText;
    public Image boughtImage;
}

public class PurchagePopUpScreen : UIBase
{
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one;
    private RectTransform rectTransform;

    [SerializeField] private Button crossButton;
    [SerializeField] private List<WeaponUI> weaponUIList;


    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
    }

    private void UpdateStoreData()
    {
        var weaponData = GameManager.Instance.weaponDataList;
        foreach (var weapon in weaponData)
        {
            var weaponUI = weaponUIList.Find(x => x.weapon == weapon.weapon);
            if (weapon.isBought)
            {
                weaponUI.priceText.text = "It's Yours!";
                weaponUI.boughtImage.gameObject.SetActive(true);
                weaponUI.buttonText.text = "PLACE";
            }
            else
            {
                weaponUI.boughtImage.gameObject.SetActive(false);
                weaponUI.priceText.text = weapon.price + " Coins";
                weaponUI.buttonText.text = "BUY";
            }
        }
    }

    private void AddListeners()
    {
        foreach (var weaponUI in weaponUIList)
        {
            weaponUI.button.onClick.AddListener(OnBuyButtonClick(weaponUI));
        }

        crossButton.onClick.AddListener(GameManager.Instance.ResumeGame);
    }

    private UnityAction OnBuyButtonClick(WeaponUI weapon)
    {
        return () =>
        {
            // Find the index instead of the item
            int weaponIndex = GameManager.Instance.weaponDataList.FindIndex(x => x.weapon == weapon.weapon);
            if (weaponIndex != -1)
            {
                var weaponData = GameManager.Instance.weaponDataList[weaponIndex];

                if (weaponData.isBought)
                {
                    GameManager.Instance.PlacePillarWeapon(weaponData.weapon);
                }
                else
                {
                    if (GameManager.Instance.GetCoins >= weaponData.price)
                    {
                        GameManager.Instance.SpendCoins(weaponData.price);
                        weaponData.isBought = true;
                        // Update the actual list item
                        GameManager.Instance.weaponDataList[weaponIndex] = weaponData;
                        UpdateStoreData();
                        Debug.Log("Weapon Bought!");
                    }
                    else
                    {
                        Debug.Log("Not enough coins to purchase weapon!");
                    }
                }
            }
        };
    }

    private IEnumerator AnimatePanel(bool show, UnityAction onComplete)
    {
        float elapsed = 0f;

        Vector3 fromScale = show ? startScale : endScale;
        Vector3 toScale = show ? endScale : startScale;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = animationCurve.Evaluate(elapsed / animationDuration);

            // Scale animation
            rectTransform.localScale = Vector3.Lerp(fromScale, toScale, progress);


            yield return null;
        }
        
        rectTransform.localScale = toScale;
        onComplete?.Invoke();
    }

    private void RemoveListeners()
    {
        foreach (var weaponUI in weaponUIList)
        {
            weaponUI.button.onClick.RemoveAllListeners();
        }

        crossButton.onClick.RemoveListener(GameManager.Instance.ResumeGame);
    }

    public override void Show()
    {
        UpdateStoreData();
        base.Show();
        StartCoroutine(AnimatePanel(true, () =>
        {
            GameManager.Instance.PauseGame();
        }));
        AddListeners();
    }

    public override void Hide()
    {
        StartCoroutine(AnimatePanel(false, () =>
        {
            RemoveListeners();
            base.Hide();
        }));
    }
}