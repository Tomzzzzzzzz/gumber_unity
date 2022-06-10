using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class InvitationManager : MonoBehaviour
{
    public GameObject panel;
    public GameObject choice1, choice2;
    public TextMeshProUGUI pseudo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Accept()
    {
        Hide();
        PlayerData localPlayer = NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>();
        PlayerData[] group = GameObject.Find("GroupCanvas").GetComponent<GroupManager>().playersInGroup;
        int i = 0;
        while (i < 4 && group[i] != null)
        {
            Debug.Log($"Appel sur AcceptInvitation du {i}eme joueur du groupe");
            NetworkIdentity identity = group[i].GetComponentInParent<NetworkIdentity>();
            localPlayer.AcceptInvitation(identity, localPlayer);
            i++;
        }
        if(i < 4)
        {
            group[i] = localPlayer;
        }
    }

    public void Refuse()
    {
        Hide();
        PlayerData[] group = GameObject.Find("GroupCanvas").GetComponent<GroupManager>().playersInGroup;
        group = new PlayerData[4];
        group[0] = NetworkClient.localPlayer.gameObject.GetComponent<PlayerData>();
    }
    private void Hide()
    {
        panel.transform.position = new Vector3(0,0,0);
    }
}
