using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour, IBuilding
{
    public string name = "Building";
    public DropList dropList = DropList.Weaponsmith;
    
    public Button openOverlayButton;

    public Canvas overlay;
    public TextMeshProUGUI overlayTitle;
    public TextMeshProUGUI overlayContent;
    public Button upgradeButton;
    public Button closeOverlayButton;
    public Button closeOverlayBackgroundButton;

    public List<GameObject> levels;
    public GameObject currentPrefab;

    private DropPool<Item> _dropPool;
    private int _level;
    private bool _isOverlayOpen;

    private readonly Dictionary<DropList, List<Item>> _dropPools = new Dictionary<DropList, List<Item>>()
    {
        {
            DropList.Weaponsmith, new List<Item>()
            {
                new Weapon
                {
                    Attack = 1,
                    Name = "Stick",
                    Rank = Rank.Common,
                    Value = 10
                },
                new Weapon
                {
                    Attack = 2,
                    Name = "Big Stick",
                    Rank = Rank.Rare,
                    Value = 20
                }
            }
        },
        {
            DropList.Armorsmith, new List<Item>()
            {
                new Armor()
                {
                    Defence = 1,
                    Name = "Shield",
                    Rank = Rank.Common,
                    Value = 10
                },
                new Armor
                {
                    Defence = 2,
                    Name = "Big Shield",
                    Rank = Rank.Rare,
                    Value = 20
                }
            }
        }
    };
    
    public void Awake()
    {
        _dropPool = new DropPool<Item>(_dropPools[dropList]);
        SetupGui();
        UpdatePrefab();
    }

    public List<Item> GetItems()
    {
        return _dropPool.CurrentItems;
    }

    public void UpdateItems()
    {
        var dropPoolSize = 10 + _level;
        _dropPool.UpdateCurrentItems(dropPoolSize);
        UpdateOverlay();
    }

    public void Upgrade()
    {
        if (_level >= levels.Count - 1) return;

        _level += 1;
        UpdateItems();
        UpdatePrefab();
        UpdateOverlay();
    }

    private void UpdatePrefab()
    {
        if (currentPrefab != null) Destroy(currentPrefab);

        var prefab = levels[_level];
        currentPrefab = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        
    }

    private void SetupGui()
    {
        // In-world
        var worldCanvas = GetComponentInChildren<Canvas>();
        worldCanvas.worldCamera = Camera.main;
        var openOverlayButtonText = openOverlayButton.GetComponentInChildren<TextMeshProUGUI>();
        openOverlayButtonText.text = name;
        openOverlayButton.onClick.AddListener(OpenOverlay);

        // Overlay
        overlayTitle.text = name;
        upgradeButton.onClick.AddListener(Upgrade);
        closeOverlayButton.onClick.AddListener(CloseOverlay);
        closeOverlayBackgroundButton.onClick.AddListener(CloseOverlay);
    }

    public void OpenOverlay()
    {
        if (_isOverlayOpen) return;
        UpdateOverlay();
        Camera.main.GetComponent<FreeFlyCamera>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        overlay.enabled = true;
        _isOverlayOpen = true;
    }

    private void UpdateOverlay()
    {
        upgradeButton.enabled = _level < levels.Count - 1;

        if (!upgradeButton.enabled)
        {
            upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Max Level";
        }

        overlayTitle.text = $"{name} - Level {_level}";

        var textContent = string.Join(Environment.NewLine,
            GetItems().Select(item => $"{item.Rank} {item.Name} - ${item.Value}"));

        overlayContent.text = textContent;
    }

    private void CloseOverlay()
    {
        overlay.enabled = false;
        _isOverlayOpen = false;
        Camera.main.GetComponent<FreeFlyCamera>().enabled = true;
    }
}
