using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class GroupManager : NetworkBehaviour
{
    public PlayerData[] playersInGroup;
    public static int nbPlayers;
    public GameObject playerInfo, playersToInvite, parentinfo, parentinvite, panelGroup;
    public static GroupManager instance;
    public static bool isShown = false;
    public Sprite Assassin_h;
    public Sprite Hunter_h;
    public Sprite Support_h;
    public Sprite Paladin_h;
    public Sprite Mage_h;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        panelGroup.SetActive(false);
        if (!isServer)
        {
            nbPlayers = 1;
            playersInGroup = new PlayerData[4];
            playersInGroup[0] = NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>();
        }
        /*
        if (!isServer)
        {
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            NetworkIdentity client_id = NetworkClient.localPlayer;
            Debug.Log("Trying to assign authority");
            AssignAuthority(identity, client_id);
            //NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>().
        }
        */
        
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
                int j = 0;
                while (j < 4 && playersInGroup[j] != null)
                {
                    GameObject slot = Instantiate(playerInfo, parentinfo.transform.position, transform.rotation);
                    slot.transform.SetParent(parentinfo.transform);

                    TextMeshProUGUI pseudo = slot.transform.Find("Pseudo").GetComponent<TextMeshProUGUI>();
                    pseudo.text = playersInGroup[j].pseudo;

                    Image tete = slot.transform.Find("TeteJoueur").GetComponent<Image>();
                    switch (playersInGroup[j].headSprite.name)
                    {
                        case "Paladin_h":
                        {
                            tete.sprite = Paladin_h;
                            break;
                        }
                        case "assassin_h":
                        {
                            tete.sprite = Assassin_h;
                            break;
                        }
                        case "hunter_h":
                        {
                            tete.sprite = Hunter_h;
                            break;
                        }
                        case "Support_h":
                        {
                            tete.sprite = Support_h;
                            break;
                        }
                        case "Mage_h":
                        {
                            tete.sprite = Mage_h;
                            break;
                        }
                    }

                    Sprite head = slot.transform.Find("TeteJoueur").GetComponent<Image>().sprite;
                    head = playersInGroup[j].headSprite;
                    j++;
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

                    slot.GetComponent<RandomInfoID>().id = i;

                    Image tete = slot.transform.Find("TeteJoueur").GetComponent<Image>();
                    switch (GameManager.instance.closePlayers[i].headSprite.name)
                    {
                        case "Paladin_h":
                        {
                            tete.sprite = Paladin_h;
                            break;
                        }
                        case "assassin_h":
                        {
                            tete.sprite = Assassin_h;
                            break;
                        }
                        case "hunter_h":
                        {
                            tete.sprite = Hunter_h;
                            break;
                        }
                        case "Support_h":
                        {
                            tete.sprite = Support_h;
                            break;
                        }
                        case "Mage_h":
                        {
                            tete.sprite = Mage_h;
                            break;
                        }
                    }
                }
            }
        }
    }

    public void Hide()
    {
        panelGroup.SetActive(false);
        isShown = false;
    }

    /*
    [Command]
    public void AssignAuthority(NetworkIdentity identity, NetworkIdentity client_id)
    {
        Debug.Log("Trying to assign authority from mthode");
        if (!identity.hasAuthority)
        {
            identity.RemoveClientAuthority();
            identity.AssignClientAuthority(client_id.connectionToClient);
            Debug.Log("Authority assigned");
        }

    }
    */

}