using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public GameObject slapPrint;
    public GameObject stunnedSymbol;
    public GameObject enemyRagdoll;
    public float ragdollSpeed = 10;

    HealBars healBars;

    EnemyAnimator anim;
    GameManager gameManager;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        anim = EnemyAnimator.instance;
        gameManager = GameManager.instance;
        healBars = HealBars.instance;
    }
    public void PlayEnemy()
    {
        gameManager.myTurn = false;
        if (!gameManager.myTurn)
        {
            //kamera ilk konumuna gel
            CameraController.instance.ChangeCamera(0);

            PaperControl.instance.GivePaper(false);
            StartCoroutine(ThrowPaper());
            //dönerse tokat at 50 ila 10 arasýnda hasar ver sýra playera geçsin
            //dönmezse sýra playera geçsin

        }
    }
    IEnumerator ThrowPaper()
    {
        yield return new WaitForSeconds(1);
        anim.SetAnim("Throw", true);
        StartCoroutine(EndAnims());
    }
    public void Slap()
    {
        anim.SetAnim("Slap", true);
        CharacterController.instance.StunAnim(true);
        PaperControl.instance.RestartPaper();
        StartCoroutine(EndAnims(true));
    }
    IEnumerator EndAnims(bool isSlap = false)
    {
        yield return new WaitForSeconds(1f);
        anim.SetAnim("Throw", false);
        anim.SetAnim("Slap", false);

        if (isSlap)
        {
            float targetHeal = healBars.GetBarValue(true) - Random.Range(1, 6) * .1f;
            healBars.SetBarValue(true, targetHeal);
            if (targetHeal <= .09f)
            {
                CharacterController.instance.EnableRagdoll();
                anim.SetAnim("Win", true);
                yield return new WaitForSeconds(1);
                MenuController.instance.LosePanel();
            }
            else
            {
                CharacterController.instance.slapPrint.SetActive(true);
                CharacterController.instance.stunnedSymbol.GetComponent<Animator>().SetBool("Start", true);
                yield return new WaitForSeconds(1);
                CharacterController.instance.PlayPlayer();
                CharacterController.instance.StunAnim(false);
                yield return new WaitForSeconds(1f);
                CharacterController.instance.stunnedSymbol.GetComponent<Animator>().SetBool("Start", false);
            }

        }
    }
    public void EnableRagdoll()
    {
        enemyRagdoll.SetActive(true);
        enemyRagdoll.GetComponent<Rigidbody>().AddForce(Vector3.forward * ragdollSpeed * Time.deltaTime);
        gameObject.SetActive(false);
    }
}
