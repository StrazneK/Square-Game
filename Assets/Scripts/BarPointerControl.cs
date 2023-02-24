using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarPointerControl : MonoBehaviour
{
    [SerializeField] Transform powerBar;
    [SerializeField] Material barMat;
    [SerializeField] Transform barTriggers;
    public BarCircle barCircle;
    public static BarPointerControl instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        float offset = 0;
        offset = Random.Range(-.18f, .19f);
        barMat.mainTextureOffset = new Vector2(offset, 0);
        barTriggers.localPosition = new Vector3(offset * 35.31111f,0 , 0);
    }
    public void TapScreen(int stageId) //0 yeþil 1 sarý 2 kýrmýzý
    {
        powerBar.GetComponent<Animator>().enabled = false;
        StartCoroutine(DisableObj(stageId));
    }
    IEnumerator DisableObj(int stageId)
    {
        yield return new WaitForSeconds(.3f);
        powerBar.gameObject.SetActive(false);
        if (stageId == 0 || stageId == 1)
        {
            CharacterController.instance.ThrowPaper();
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
