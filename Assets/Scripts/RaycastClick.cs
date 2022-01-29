using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaycastClick : MonoBehaviour
{
    public Sprite hoverSprite;
    public Image cursorImage;
    public TextMeshProUGUI cursorText;

    public LayerMask layerMask;

    private Sprite _initialCursorSprite;

    public void Awake()
    {
        _initialCursorSprite = cursorImage.sprite;
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100, layerMask))
        {
            var building = hit.transform.gameObject.GetComponent<Building>();
            cursorText.text = building.displayName;
            cursorImage.sprite = hoverSprite;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                building.OpenOverlay();
            }
        }
        else
        {
            cursorText.text = null;
            cursorImage.sprite = _initialCursorSprite;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.position + transform.forward * 1000);
    }
}
