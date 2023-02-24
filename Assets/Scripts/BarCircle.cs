using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarCircle : MonoBehaviour
{
    int barCount = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BarHigh")
        {
            SetBarCount(int.Parse(other.name));
        }
        else if (other.tag == "GreenBar")
        {
            SetBarCount(0);
            BarControl.instance.barCircle.SetBarCount(5);
        }
        else if (other.tag == "YellowBar")
        {
            SetBarCount(1);
            BarControl.instance.barCircle.SetBarCount(4);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "YellowBar" && barCount == 1) //eðer sarýdan çýktýysa ve en son girdiði yer sarýysa (sarýdan yeþile geçiþte koþulun çalýþmasýný engellemek için)
        {
            SetBarCount(2); //bar kýrmýzýdadýr
            BarControl.instance.barCircle.SetBarCount(1);
        }
    }
    public void SetBarCount(int newCount)
    {
        barCount = newCount;
    }
    public int GetBarCount()
    {
        return barCount;
    }
}
