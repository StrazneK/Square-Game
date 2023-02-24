using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Animator anim;
    public static CharacterController instance;
    HealBars healBars;

    public GameObject slapPrint;
    public GameObject stunnedSymbol;
    public GameObject playerRagdoll;

    EnemyController enemyController;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        enemyController = EnemyController.instance;
        anim = transform.GetChild(0).GetComponent<Animator>();
        healBars = HealBars.instance;
    }

    public void ThrowPaper()
    {
        anim.SetBool("Throw", true);
        StartCoroutine(EndAnims());
    }
    public void Slap()
    {
        anim.SetBool("Slap", true);
        EnemyAnimator.instance.SetAnim("Stun", true);
        PaperControl.instance.RestartPaper();
       // healBars.SetBarValue(false, healBars.GetBarValue(false) - BarCircle.instance.GetBarCount() * .1f);
        StartCoroutine(EndAnims(true));
    }
    public void FailAnim(bool isTrue)
    {
        anim.SetBool("Loose", isTrue);
    }
    public void StunAnim(bool isTrue)
    {
        anim.SetBool("Stun", isTrue);
    }
    public void PlayPlayer()
    {
        GameManager.instance.myTurn = true; 
        TapControl.instance.StartGame();
    }
    IEnumerator EndAnims(bool isSlap = false)

    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool("Throw", false);
        anim.SetBool("Slap", false);
        if (isSlap)
        {
            float targetHeal = healBars.GetBarValue(false) - BarControl.instance.barCircle.GetBarCount() * .1f;
            healBars.SetBarValue(false, targetHeal);
            if (targetHeal <= .09f)
            {
                CameraController.instance.PlayConfetti(true);
                enemyController.EnableRagdoll();
                anim.SetBool("Win", true);
                yield return new WaitForSeconds(1);
                MenuController.instance.WinPanel();
            }
            else
            {
                EnemyController.instance.slapPrint.SetActive(true);
                EnemyController.instance.stunnedSymbol.GetComponent<Animator>().SetBool("Start", true);
                yield return new WaitForSeconds(1);
                enemyController.PlayEnemy();
            }

        }
        EnemyAnimator.instance.SetAnim("Stun", false);
        yield return new WaitForSeconds(1f);
        EnemyController.instance.stunnedSymbol.GetComponent<Animator>().SetBool("Start", false);
    }
    public void EnableRagdoll()
    {
        playerRagdoll.SetActive(true);
        gameObject.SetActive(false);
    }
}
