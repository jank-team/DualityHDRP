using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class PlaceableBuilding
{
    public GameObject prefab;
    public int cost;
    public bool ignoreCollision;
}

public class RaycastClick : MonoBehaviour
{
    public Sprite invalidBuildingPlacementCursorImage;
    public Sprite hoverSprite;
    public Image cursorImage;
    public TextMeshProUGUI cursorText;
    public TextMeshProUGUI instructionText;

    public LayerMask groundMask;
    public LayerMask invalidBuildingPlacementMask;

    [FormerlySerializedAs("layerMask")] public LayerMask clicklayerMask;
    public LayerMask entityLayerMask;

    public int reach = 150;

    public List<PlaceableBuilding> buildings;
    private GameObject _building;

    private BoxCollider _buildingCollider;
    private int _buildingIndex;

    private Sprite _initialCursorSprite;
    private bool _isBuildingMode;

    public void Awake()
    {
        _initialCursorSprite = cursorImage.sprite;
    }

    // Update is called once per frame
    private void Update()
    {
        var action = _isBuildingMode ? "exit" : "enter";
        instructionText.text = $"Right click to {action} building mode.";
        if (!CheckPlaceBuilding() && !CheckClickBuilding() && !CheckInspectEntity())
        {
            cursorText.text = null;
            cursorImage.sprite = _initialCursorSprite;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.position + transform.forward * 1000);
    }

    private bool CheckPlaceBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) _isBuildingMode = !_isBuildingMode;

        if (_isBuildingMode)
        {
            var isCtrlDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            var mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");

            var isBuildingChanged = false;

            if (isCtrlDown && _building)
            {
                if (mouseScroll > 0.1)
                    _building.transform.Rotate(Vector3.up, 5);
                else if (mouseScroll < -0.1) _building.transform.Rotate(Vector3.up, -5);
            }
            else if (!isCtrlDown)
            {
                if (mouseScroll > 0.1)
                {
                    _buildingIndex += 1;
                    isBuildingChanged = true;
                    if (_buildingIndex == buildings.Count()) _buildingIndex = 0;
                }
                else if (mouseScroll < -0.1)
                {
                    _buildingIndex -= 1;
                    isBuildingChanged = true;
                    if (_buildingIndex < 0) _buildingIndex = buildings.Count - 1;
                }
            }

            var placeableBuilding = buildings[_buildingIndex];


            var hasGroundHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out var groundHit, reach, groundMask);

            if (isBuildingChanged || _building == null)
            {
                if (_building != null) Destroy(_building);
                _building = Instantiate(placeableBuilding.prefab, Vector3.down * 1000, Quaternion.identity);
                _buildingCollider = _building.GetComponent<BoxCollider>();
                if (_buildingCollider != null) _buildingCollider.enabled = false;
            }

            if (hasGroundHit) _building.transform.position = groundHit.point;

            if (!hasGroundHit || !placeableBuilding.ignoreCollision && Physics.CheckBox(groundHit.point,
                _buildingCollider.size / 2, Quaternion.identity,
                invalidBuildingPlacementMask))
            {
                cursorText.text = "You can't build here.";
                cursorImage.sprite = invalidBuildingPlacementCursorImage;
                return true;
            }

            // Valid building placement
            cursorImage.sprite = _initialCursorSprite;
            cursorText.text = $@"${placeableBuilding.cost}

Left click to place building.

Mouse wheel to change building.

CTRL + Mouse Wheel to rotate.";

            if (Town.Instance.balance < placeableBuilding.cost)
            {
                cursorImage.sprite = invalidBuildingPlacementCursorImage;
                return true;
            }

            if (!Input.GetKeyDown(KeyCode.Mouse0)) return true;

            // Place building
            if (_buildingCollider != null) _buildingCollider.enabled = true;
            _buildingCollider = null;
            _building = null;

            Town.Instance.balance -= placeableBuilding.cost;

            return true;
        }

        if (_building != null) Destroy(_building);

        return false;
    }

    private bool CheckClickBuilding()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, reach,
            clicklayerMask))
        {
            var building = hit.transform.gameObject.GetComponent<Building>();

            if (building is null) return false;

            cursorText.text = building.displayName;
            cursorImage.sprite = hoverSprite;

            if (Input.GetKeyDown(KeyCode.Mouse0)) building.OpenOverlay();

            return true;
        }

        return false;
    }

    private bool CheckInspectEntity()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, reach,
            entityLayerMask))
        {
            var entity = hit.transform.gameObject.GetComponent<Entity>();

            if (entity is null) return false;

            cursorText.text = $@"{entity.GetDisplayName()}

{entity.GetDescription()}
";
            cursorImage.sprite = _initialCursorSprite;

            return true;
        }

        return false;
    }
}
