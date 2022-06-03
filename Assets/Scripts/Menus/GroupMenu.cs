using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupMenu : MonoBehaviour
{
    public static bool isShown = false;
    public GameObject groupMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isShown)
            {
                QuitMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void QuitMenu()
    {
        groupMenuUI.SetActive(false);
        isShown = false;
    }

    void OpenMenu()
    {
        groupMenuUI.SetActive(true);
        isShown = true;
    }
}
