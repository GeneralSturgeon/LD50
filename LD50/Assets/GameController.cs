using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [HideInInspector]
    public float currentDistance;
    private int currentReward;
    public int currency = 0;
    [SerializeField]
    private Animator crashPanelAnim;
    [SerializeField]
    private TextMeshProUGUI continousDistanceText;
    [SerializeField]
    private TextMeshProUGUI menuDistanceText;
    [SerializeField]
    private TextMeshProUGUI rewardText;
    private const float DISTANCE_MULTIPLIER = 2;
    [SerializeField]
    private UpgradeSlot[] upgradeSlots;
    [SerializeField]
    private TextMeshProUGUI currencyText;
    [SerializeField]
    private Spawner spawner;
    [HideInInspector]
    public int remainingPokes;
    [SerializeField]
    private PokesPanel pokesPanel;
    [SerializeField]
    private PickupSpawner pickupSpawner;

    public int[] pokes;
    public float[] startingForce;
    public float[] lift;
    public float[] frontWeight;
    public float[] aOA;

    [HideInInspector]
    public int pokesUpgrades = 0;
    public int startingForceUpgrades = 0;
    public int liftUpgrades = 0;
    public int frontWeightUpgrades = 0;
    public int aOAUpgrades = 0;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        remainingPokes = pokes[pokesUpgrades];
        UpdatePokes();
    }


    public void OpenCrashPanel()
    {

        crashPanelAnim.SetTrigger("In");
        crashPanelAnim.ResetTrigger("Out");
        currentReward = Mathf.FloorToInt(currentDistance / DISTANCE_MULTIPLIER);
        currency += currentReward;
        UpdateUI();
    }

    public void UpdateUI()
    {
        continousDistanceText.text = (Mathf.FloorToInt(currentDistance)).ToString() + " m";
        menuDistanceText.text = (Mathf.RoundToInt(currentDistance)).ToString() + " m";
        rewardText.text = currentReward.ToString() + " $";

        foreach(UpgradeSlot slot in upgradeSlots)
        {
            slot.UpdateUI();
        }

        currencyText.text = currency.ToString() + " $";
    }

    public void UpdateDistance(float distance)
    {
        currentDistance = distance/DISTANCE_MULTIPLIER;
        UpdateUI();
    }

    public void RemoveCurrency(int _currency)
    {
        currency -= _currency;
    }

    public void Restart()
    {
        crashPanelAnim.SetTrigger("Out");
        crashPanelAnim.ResetTrigger("In");
        currentDistance = 0f;
        StateManager.instance.currentGameState = StateManager.GameState.Menu;
        remainingPokes = pokes[pokesUpgrades];
        UpdatePokes();
        pickupSpawner.SpawnNewPickups();
        spawner.Spawn();

    }

    public void UpdatePokes()
    {
        pokesPanel.UpdatePanel();
    }

    public void Upgrade(UpgradeSlot.UpgradeType _type)
    {
        switch (_type)
        {
            case (UpgradeSlot.UpgradeType.Boops):
                pokesUpgrades++;
                break;

            case (UpgradeSlot.UpgradeType.StartingForce):
                startingForceUpgrades++;
                break;

            case (UpgradeSlot.UpgradeType.Lift):
                liftUpgrades++;
                break;

            case (UpgradeSlot.UpgradeType.FrontWeight):
                frontWeightUpgrades++;
                break;

            case (UpgradeSlot.UpgradeType.AOA):
                aOAUpgrades++;
                break;
        }
    }
}
