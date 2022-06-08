using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CreateMMOCharacterMessage : NetworkMessage
{
    public string race;
    public string pseudo;
    //public string id;
}
