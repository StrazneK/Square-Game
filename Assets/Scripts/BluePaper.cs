using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePaper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="RedPaper")
        {
            PaperControl.instance.ThrowPaper(false, GameManager.instance.myTurn);
            TapControl.instance.TargetDisable();
        }
    }
}
