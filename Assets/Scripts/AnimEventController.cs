using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    public void ThrowPaper(int playerId) //0 biz 1 enemy
    {
        Debug.Log("AnimEventController playerid= " + playerId);
        PaperControl.instance.ThrowPaper(true, playerId == 0); //biz atýyorsak trajectory'e gidecek
    }
}
