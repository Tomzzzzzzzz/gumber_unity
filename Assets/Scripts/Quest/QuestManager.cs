using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{

    public List<QuestSO> allQuest;
    public GameObject panelQuest, descriptionPanel, parent, quest;

    public static QuestManager instance;

    bool firstQuest = true;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        panelQuest.SetActive(false);
        for (int i = 0; i < allQuest.Count; i++)
        {
            allQuest[i].id = i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            panelQuest.SetActive(true);

            if (parent.transform.childCount > 0)
            {
                foreach (Transform child in parent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < allQuest.Count; i++)
            {
                if (i <= allQuest.Count -1)
                {
                    if (allQuest[i].statut == QuestSO.Statut.accepter)
                    {
                        GameObject slot = Instantiate(quest, parent.transform.position, transform.rotation);
                        slot.transform.SetParent(parent.transform);

                        TextMeshProUGUI title = slot.transform.Find("TitleQuest").GetComponent<TextMeshProUGUI>();
                        title.text = allQuest[i].title;

                        TextMeshProUGUI statut = slot.transform.Find("Statut").GetComponent<TextMeshProUGUI>();
                        statut.text = "(" + allQuest[i].statut + ")";

                        slot.GetComponent<QuestID>().questId = allQuest[i].id;

                        if (firstQuest)
                        {
                            TextMeshProUGUI titleDes = descriptionPanel.transform.Find("TitleQuest").GetComponent<TextMeshProUGUI>();
                            titleDes.text = allQuest[i].title;

                            TextMeshProUGUI description = descriptionPanel.transform.Find("DescriptionQuest").GetComponent<TextMeshProUGUI>();
                            description.text = allQuest[i].description;

                            TextMeshProUGUI objectif = descriptionPanel.transform.Find("Objectif").GetComponent<TextMeshProUGUI>();
                            objectif.text = "Objectif : " + allQuest[i].actualAmount + "/" + allQuest[i].amountToFind;

                            TextMeshProUGUI recompense = descriptionPanel.transform.Find("Recompense").GetComponent<TextMeshProUGUI>();
                            recompense.text = "Récompense : " + allQuest[i].goldToGive;

                            firstQuest = false;
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            panelQuest.SetActive(false);
        }
    }

    public void ChargeQuest(int i)
    {
        TextMeshProUGUI titleDes = descriptionPanel.transform.Find("TitleQuest").GetComponent<TextMeshProUGUI>();
        titleDes.text = allQuest[i].title;

        TextMeshProUGUI description = descriptionPanel.transform.Find("DescriptionQuest").GetComponent<TextMeshProUGUI>();
        description.text = allQuest[i].description;

        TextMeshProUGUI objectif = descriptionPanel.transform.Find("Objectif").GetComponent<TextMeshProUGUI>();
        objectif.text = "Objectif : " + allQuest[i].actualAmount + "/" + allQuest[i].amountToFind;

        TextMeshProUGUI recompense = descriptionPanel.transform.Find("Recompense").GetComponent<TextMeshProUGUI>();
        recompense.text = "Récompense : " + allQuest[i].goldToGive;

    }
}
