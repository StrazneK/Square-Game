using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarControl : MonoBehaviour
{
    [SerializeField] Transform powerBar;
    public BarCircle barCircle;
    public static BarControl instance;
    private void Awake()
    {
        instance = this;
        barCircle = powerBar.GetChild(0).GetComponent<BarCircle>();
    }
    public void TapScreen(int stageId)
    {
        powerBar.GetComponent<Animator>().enabled = false;
        Debug.Log(barCircle.GetBarCount());
        StartCoroutine(DisableObj(stageId));
    }
    public string Feedback()
    {
        switch (barCircle.GetBarCount())
        {
            case 1: return "Very Bad!";
            case 2: return "Not Now";
            case 3: return "Unlucky!";
            case 4: return "Nice!";
            case 5: return "Great!";
            default: return "";
        }
    }
    public void EnabledObj()
    {
        powerBar.gameObject.SetActive(true);
        powerBar.GetComponent<Animator>().enabled = true;
    }

    public void PowerBarUnvisible()
    {
        powerBar.gameObject.SetActive(false);
    }
    IEnumerator DisableObj(int stageId)
    {
        yield return new WaitForSeconds(.3f);
        powerBar.gameObject.SetActive(false);
        if (stageId == 1)
        {
            CharacterController.instance.ThrowPaper();
        }
        else if (stageId == 2)
        {
            CharacterController.instance.Slap();
        }
    }
    public void RestartBar()
    {
        powerBar.gameObject.SetActive(true);
        powerBar.GetComponent<Animator>().enabled = true;
    }
}
