using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    bool inTargetArea = false;
    public static TargetControl instance;
    private void Awake()
    {
        instance = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TargetArea")
            inTargetArea = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TargetArea")
            inTargetArea = false;
    }
    public bool IsInTarget()
    {
        return inTargetArea;
    }
}
