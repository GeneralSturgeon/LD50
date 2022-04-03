using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public enum UpgradeType {Boops, StartingForce, Lift, FrontWeight, AOA}

    [SerializeField]
    private int[] costs;
    [SerializeField]
    private int currentUpgrade = 0;
    [SerializeField]
    private Button btn;
    [SerializeField]
    private UpgradeType upgradeType;
    [SerializeField]
    private TextMeshProUGUI costText;
    [SerializeField]
    private Image[] dots;
    [SerializeField]
    private Color newColor;

    public void UpdateUI()
    {
        if(GameController.instance.currency >= costs[currentUpgrade] && currentUpgrade < costs.Length)
        {
            btn.interactable = true;
        } else
        {
            btn.interactable = false;
        }

        
        if(currentUpgrade == costs.Length -1)
        {
            costText.text = "Max";
        } else
        {
            costText.text = costs[currentUpgrade].ToString() + " $";
        }
        
    }

    public void Upgrade()
    {
        GameController.instance.PlayClick();
        GameController.instance.Upgrade(upgradeType);
        GameController.instance.RemoveCurrency(costs[currentUpgrade]);
        dots[currentUpgrade].color = newColor;
        currentUpgrade++;
        GameController.instance.UpdateUI();
    }
}
