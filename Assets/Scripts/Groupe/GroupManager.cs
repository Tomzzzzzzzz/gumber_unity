using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class GroupManager : NetworkBehaviour
{
    public List<PlayerData> playersInGroup;
    public static int nbPlayers;
    public GameObject playerInfo, playersToInvite, parentinfo, parentinvite, panelGroup;
    public static GroupManager instance;
    public static bool isShown = false;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        panelGroup.SetActive(false);
        nbPlayers = 1;
        playersInGroup = new List<PlayerData> {PlayerData.instance};
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isShown)
            {
                Hide();
            }
            else
            {
                QuestManager.instance.Hide();
                panelGroup.SetActive(true);
                isShown = true;
                if (parentinfo.transform.childCount > 0)
                {
                    foreach (Transform child in parentinfo.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                if (parentinvite.transform.childCount > 0)
                {
                    foreach (Transform child in parentinvite.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                //                 //
                //  Panel members  //
                //                 //
                for (int i = 0; i < playersInGroup.Count; i++)
                {
                    GameObject slot = Instantiate(playerInfo, parentinfo.transform.position, transform.rotation);
                    slot.transform.SetParent(parentinfo.transform);

                    TextMeshProUGUI pseudo = slot.transform.Find("Pseudo").GetComponent<TextMeshProUGUI>();
                    pseudo.text = playersInGroup[i].pseudo;

                    Sprite head = slot.transform.Find("TeteJoueur").GetComponent<Sprite>();
                    head = playersInGroup[i].headSprite;
                }

                //                 //
                //  Panel invite   //
                //                 //
                for (int i = 0; i < GameManager.instance.closePlayers.Count; i++)
                {
                    GameObject slot = Instantiate(playersToInvite, parentinvite.transform.position, transform.rotation);
                    slot.transform.SetParent(parentinvite.transform);

                    TextMeshProUGUI pseudo = slot.transform.Find("Pseudo").GetComponent<TextMeshProUGUI>();
                    pseudo.text = GameManager.instance.closePlayers[i].pseudo;

                    Sprite head = slot.transform.Find("TeteJoueur").GetComponent<Sprite>();
                    head = GameManager.instance.closePlayers[i].headSprite;

                    slot.GetComponent<RandomInfoID>().id = GameManager.instance.closePlayers[i].id;
                }
            }
        }
    }

    public void Hide()
    {
        panelGroup.SetActive(false);
        isShown = false;
    }

    public void Invite()
    {

    }
}
