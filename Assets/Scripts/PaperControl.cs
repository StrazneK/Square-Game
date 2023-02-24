using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperControl : MonoBehaviour
{
    public static PaperControl instance;


    [SerializeField] Transform characterHand;
    [SerializeField] Transform enemyHand;
    public Transform redPaper;
    public Transform bluePaper;
    public Transform trajectoryTarget;
    bool isPlayer = true;

    bool paperThrowed = false;
    [SerializeField] float throwSpeed = 1;
    [SerializeField] float jumpSpeed = 1;

    Animator redAnim, blueAnim;

    Rigidbody bRb, rRb;

    BarCircle barCircle;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        redAnim = redPaper.GetComponent<Animator>();
        blueAnim = bluePaper.GetComponent<Animator>();
        bRb = bluePaper.GetComponent<Rigidbody>();
        rRb = redPaper.GetComponent<Rigidbody>();
        barCircle = BarControl.instance.barCircle;
        Debug.Log(barCircle.name);
        GivePaper(true);
    }

    private void Update()
    {
        if (paperThrowed)
        {
            if (isPlayer) //eðer bizdeyse trajectorye doðru git
                bluePaper.position = Vector3.Lerp(bluePaper.position, trajectoryTarget.position, throwSpeed * Time.deltaTime);
            else //eðer kýrmýzý kaðýda doðru gidecekse
                bluePaper.position = Vector3.Lerp(bluePaper.position, redPaper.position, throwSpeed * Time.deltaTime);
        }
    }
    public void GivePaper(bool isCharacter)
    {
        bRb.useGravity = false;
        bRb.isKinematic = true;
        bluePaper.GetComponent<BoxCollider>().enabled = true;
        bluePaper.parent = isCharacter ? characterHand : enemyHand;
        bluePaper.localEulerAngles = bluePaper.localPosition = Vector3.zero;
    }
    public void ThrowPaper(bool start, bool _targetTraj)
    {
        bluePaper.parent = null;
        paperThrowed = start;
        isPlayer = _targetTraj;
        if (!start)
        {
            bluePaper.GetComponent<BoxCollider>().enabled = false;
            int powerCount = 0;
            if (_targetTraj) //eðer biz atýyorsak
                powerCount = barCircle.GetBarCount(); //deðeri bardan al
            else //eðer enemy atýyorsa
                powerCount = Random.Range(3, 6); //deðeri rastgele al
            Debug.Log("Bar deðeri= " + barCircle.GetBarCount());
            Debug.Log("powerCount= " + powerCount);
            RedPaperFlip(powerCount);
        }
    }
    public void RedPaperFlip(int power)
    {
        if (power >= 4)
        {
            redAnim.SetBool("Flip", true);
            redAnim.SetInteger("FlipId", Random.Range(0, 4));
            StartCoroutine(EndFlip(true));
        }
        else
        {
            redAnim.SetBool("Jump", true);
            redAnim.SetInteger("JumpId", Random.Range(0, 4));
            StartCoroutine(EndFlip(false));
        }
        bRb.useGravity = true;
        bRb.isKinematic = false;
        bRb.AddForce(Vector3.one * jumpSpeed);
    }
    public void RestartPaper()
    {
        redAnim.SetBool("Flip", false);
    }
    IEnumerator EndFlip(bool isFlip)
    {
        if (isFlip)
        {
            yield return new WaitForSeconds(.7f);
            redAnim.SetBool("Jump", false);
            CameraController.instance.PlayConfetti(false);
            if (isPlayer)
            {
                CameraController.instance.ChangeCamera(2);
                EnemyAnimator.instance.SetAnim("Loose", true);
                yield return new WaitForSeconds(.5f);
                TapControl.instance.ChangeStage(2);
                BarControl.instance.RestartBar();
                EnemyAnimator.instance.SetAnim("Loose", false);
            }
            else
            {
                CameraController.instance.ChangeCamera(3);
                CharacterController.instance.FailAnim(true);
                yield return new WaitForSeconds(.5f);
                CharacterController.instance.FailAnim(false);
                yield return new WaitForSeconds(.5f);
                HealBars.instance.OpenCloseHealBars(true); //barlarý aç
                yield return new WaitForSeconds(.5f);
                EnemyController.instance.Slap();
            }
            // redAnim.SetBool("Flip", false);
        }
        else
        {
            yield return new WaitForSeconds(1);
            redAnim.SetBool("Jump", false);
            if (isPlayer)
            {
                CharacterController.instance.FailAnim(true);
                EnemyController.instance.PlayEnemy();
                yield return new WaitForSeconds(.8f);
                CharacterController.instance.FailAnim(false);
            }
            else
            {
                EnemyAnimator.instance.SetAnim("Loose", true);
                yield return new WaitForSeconds(.8f);
                CharacterController.instance.PlayPlayer();
                EnemyAnimator.instance.SetAnim("Loose", false);
            }
            yield return new WaitForSeconds(.8f);
        }

    }
}
