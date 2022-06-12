using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Mirror;
public class PNJ : NetworkBehaviour
{
    [SerializeField]
    string[] sentences;
    [SerializeField]
    string characterName;
    int index;
    bool isOndial, canDial;

    HUDManager manager => HUDManager.instance;

    public QuestSO quest;

    public Sprite iconQuest1, iconQuest2;
    public SpriteRenderer questSr;

    public GameObject choice1, choice2;

    private void Start()
    {
        choice1 = manager.choice1;
        choice2 = manager.choice2;
        if (quest != null && quest.statut == QuestSO.Statut.none)
        {
            questSr.sprite = iconQuest1;
        }
        
        else if (quest == null)
        {
            questSr.sprite = null;
        }
    }

    private void Update()
    {
        if (isOndial)
        {
            Time.timeScale = 0f;
        }

        if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount < quest.amountToFind)
        {
            questSr.sprite = iconQuest2;
            questSr.color = Color.red;
        } 
        else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount >= quest.amountToFind)
        {
            questSr.sprite = iconQuest2;
            questSr.color = Color.yellow;
        }
        else if (quest != null && quest.statut == QuestSO.Statut.complete)
        {
            questSr.sprite = null;
        } 
        

        if (Input.GetKeyDown(KeyCode.E) && canDial)
        {
            if (quest != null && quest.statut == QuestSO.Statut.none)
            {
                StartDialogue(quest.sentences);
            }
            else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount < quest.amountToFind)
            {
                StartDialogue(quest.InprogressSentence);
            } 
            else if (quest != null && quest.statut == QuestSO.Statut.accepter && quest.actualAmount >= quest.amountToFind)
            {
                StartDialogue(quest.completeSentence);
                quest.statut = QuestSO.Statut.complete;

                PlayerData.instance.AddXp(200);  //récompense quête

                foreach (var item in QuestManager.instance.allQuest) //détruire objet quête 
                {
                    if (item.statut == QuestSO.Statut.accepter && item.objectTofind == quest.objectTofind)
                    {
                        item.actualAmount -= quest.amountToFind;
                    }
                }
            } 
            else if (quest != null && quest.statut == QuestSO.Statut.complete)
            {
                StartDialogue(quest.afterQuestSentence);
            } 
            else if (quest == null)
            {
                choice1.SetActive(false);
                choice2.SetActive(false);
                StartDialogue(sentences);
            }
        }
        if (!(isOndial) && PauseMenu.GameIsPaused == false)
        {
            Time.timeScale = 1f;
        }
    }

    public void StartDialogue(string[] sentence)
    {
        manager.dialogueHolder.SetActive(true);
        isOndial = true;
        TypingText(sentence);
        manager.continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        manager.continueButton.GetComponent<Button>().onClick.AddListener( delegate {NextLine(sentence); });
    }

    void TypingText(string[] sentence)
    {
        manager.nameDisplay.text = "";
        manager.textDisplay.text = "";

        manager.nameDisplay.text = characterName;
        manager.textDisplay.text = sentence[index];

        if (manager.textDisplay.text == sentence[index])
        {
            manager.continueButton.SetActive(true);
        }

        if (isOndial && index == sentence.Length -1)
        {
            if (quest != null && quest.statut == QuestSO.Statut.none)
                {
                    choice1.SetActive(true);
                    choice2.SetActive(true);
                }
        }
    }

    public void NextLine(string[] sentence)
    {
        manager.continueButton.SetActive(false);

        if (isOndial && index < sentence.Length - 1)
        {
            index++;
            manager.textDisplay.text = "";
            TypingText(sentence);
        }
        else
        {
            if (isOndial && index == sentence.Length -1)
            {
                isOndial = false;
                index = 0;
                manager.textDisplay.text = "";
                manager.nameDisplay.text = "";
                manager.dialogueHolder.SetActive(false);

                if (quest != null && quest.statut == QuestSO.Statut.none)
                {
                    choice1.SetActive(true);
                    choice2.SetActive(true);

                    choice1.GetComponent<Button>().onClick.RemoveAllListeners();
                    choice2.GetComponent<Button>().onClick.RemoveAllListeners();
                    choice1.GetComponent<Button>().onClick.AddListener(delegate { Accepte(); });
                    choice2.GetComponent<Button>().onClick.AddListener(delegate { Decline(); });
                }
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (isServer) {return;}
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            canDial = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (isServer) {return;}
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            canDial = false;
        }
    }

    public void Accepte()
    {
        quest.statut = QuestSO.Statut.accepter;
        isOndial = false;
        index = 0;
        manager.textDisplay.text = "";
        manager.nameDisplay.text = "";
        manager.dialogueHolder.SetActive(false);
        choice1.SetActive(false);
        choice2.SetActive(false);

    }

    public void Decline()
    {
        quest.statut = QuestSO.Statut.none;
        isOndial = false;
        index = 0;
        manager.textDisplay.text = "";
        manager.nameDisplay.text = "";
        manager.dialogueHolder.SetActive(false);
        choice1.SetActive(false);
        choice2.SetActive(false);

    }
}
