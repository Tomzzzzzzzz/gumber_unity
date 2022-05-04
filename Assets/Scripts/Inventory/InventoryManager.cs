using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public List<Items> inventory;
    public int inventoryLength = 24;
    public GameObject inventoryPanel, holderSlot;
    private GameObject slot;
    public GameObject prefabs;
    public GameObject holderDescription;

    public TextMeshProUGUI title, descriptionObject;
    public Image iconDescription;

    public static InventoryManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I) && !inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            if (holderSlot.transform.childCount > 0)
            {
                foreach (Transform item in holderSlot.transform)
                {
                    Destroy(item.gameObject);
                }
            }

            for (int i = 0; i < inventoryLength; i++)
            {

                if (i <= inventory.Count -1)
                {
                    slot = Instantiate(prefabs,transform.position,transform.rotation);
                    slot.transform.SetParent(holderSlot.transform);
                    TextMeshProUGUI amount = slot.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                    Image img = slot.transform.Find("Icon").GetComponent<Image>();
                    slot.GetComponent<Slotitem>().itemSlot = i;
                    amount.text = inventory[i].amount.ToString();
                    img.sprite = inventory[i].icon;
                }
                else if (i > inventory.Count -1)
                {
                    slot = Instantiate(prefabs,transform.position,transform.rotation);
                    slot.transform.SetParent(holderSlot.transform);
                    slot.GetComponent<Slotitem>().itemSlot = i;
                    TextMeshProUGUI amount = slot.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                    amount.gameObject.SetActive(false);
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.I) && inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(false);
        }

    }

    public void ChargeItem (int i)
    {
        holderDescription.SetActive(true);
        title.text = inventory[i].title;
        descriptionObject.text = inventory[i].description;
        iconDescription.sprite = inventory[i].icon;
    }
}
