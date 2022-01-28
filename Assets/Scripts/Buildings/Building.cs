using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class Building<T> : MonoBehaviour, IBuilding<T> where T : Item
{
    public string name = "Building";
    public Button openOverlayButton;
    public TextMeshProUGUI buttonText;

    public Canvas overlay;
    public TextMeshProUGUI overlayTitle;
    public TextMeshProUGUI overlayContent;
    public Button upgradeButton;
    public Button closeOverlayButton;
    public Button closeOverlayBackgroundButton;

    private DropPool<T> _dropPool;
    private int _level;

    public void Awake()
    {
        _dropPool = GetDropPool();
        SetupGui();
    }

    public List<T> GetItems()
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
        _level += 1;
        UpdateItems();
    }

    private void SetupGui()
    {
        // In-world
        var openOverlayButtonText = openOverlayButton.GetComponentInChildren<TextMeshProUGUI>();
        openOverlayButtonText.text = name;
        openOverlayButton.onClick.AddListener(OpenOverlay);

        // Overlay
        overlayTitle.text = name;
        upgradeButton.onClick.AddListener(Upgrade);
        closeOverlayButton.onClick.AddListener(CloseOverlay);
        closeOverlayBackgroundButton.onClick.AddListener(CloseOverlay);
    }

    private void OpenOverlay()
    {
        UpdateOverlay();
        overlay.enabled = true;
    }

    private void UpdateOverlay()
    {
        var textContent = string.Join(Environment.NewLine,
            GetItems().Select(item => $"{item.Rank} {item.Name} - ${item.Value}"));

        overlayContent.text = textContent;
    }

    private void CloseOverlay()
    {
        overlay.enabled = false;
    }

    protected abstract DropPool<T> GetDropPool();
}
