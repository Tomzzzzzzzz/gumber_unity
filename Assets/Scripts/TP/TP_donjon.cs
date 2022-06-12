using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class TP_donjon : NetworkBehaviour
{
    bool col = false;
    public PlayerController playerController;
    public GameObject player;
    [SerializeField] Vector3 vecteur;

    public static TP_donjon instance;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && col)
        {
            StartCoroutine("Teleport");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            Debug.Log("joueur entre en col");
            col = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.GetComponent<NetworkIdentity>().netId == NetworkClient.localPlayer.netId)
        {
            Debug.Log("joueur kite col");
            col = false;
        }
    }

    IEnumerator Teleport()
    {
        playerController.disabled = true;
        yield return new WaitForSeconds(0.1f);
        player.transform.position = vecteur;
        yield return new WaitForSeconds(0.1f);
        playerController.disabled = false;
    }
}
