using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        panel.SetActive(false);
        //nik zebi
    }
}
