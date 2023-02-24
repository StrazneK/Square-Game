using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TapControl : MonoBehaviour
{
    BarControl barControl;
    BarPointerControl barPointerControl;

    [SerializeField] TextMeshProUGUI fbText;
    public bool tapActive = true;
    public static TapControl instance;
    int stageId = 0;

    float distanceToScreen;
    Vector3 posMove;
    [SerializeField] Transform trajectory;
    [SerializeField] Transform trajectoryTarget;
    bool trueTarget = false;

    GameManager gameManager;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        barControl = GetComponent<BarControl>();
        barControl.PowerBarUnvisible();
        barPointerControl = GetComponent<BarPointerControl>();
        barPointerControl.PowerBarUnvisible();
        trajectory.gameObject.SetActive(false);
        trajectoryTarget.gameObject.SetActive(false);
        ChangeStage(-1);
        gameManager = GameManager.instance;
    }
    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.Play)
        {
            if (GameManager.instance.myTurn)
            {
                if (Input.GetMouseButton(0))
                {
                    if (stageId == 0) // trajectory zamaný ekrana basýlý tutuyorsa
                    {
                        distanceToScreen = Camera.main.WorldToScreenPoint(trajectoryTarget.position).z;
                        posMove = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
                        trajectoryTarget.position = new Vector3(posMove.x, -0.375f, posMove.z);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (stageId == 0) //trajectory zamaný elini basýp kaldýrdýysa
                    {
                        /*  CameraController.instance.ChangeCamera();
                          TapBar();*/
                        if (TargetControl.instance.IsInTarget()) //target doðru yerdeyse
                        {
                            trueTarget = true;
                            GiveFeedback(true);
                            ChangeStage(-1); //bir süre birþey yapamamasý için -1'e eþitle.
                            StartCoroutine(OpenBarWithSec(true));
                        }
                        else
                        {
                            trueTarget = false;
                        }
                    }
                }
                if (tapActive && Input.GetMouseButtonDown(0))
                {
                    // Time.timeScale = Time.timeScale / 4;
                    if (stageId == 1) // Kart atma
                    {
                        TapBar(true);
                        trajectory.gameObject.SetActive(false);
                    }
                    if (stageId == 2) // Tokat atma
                    {
                        TapBar();
                    }
                }
            }
        }
    }
    public void TargetDisable()
    {
        trajectoryTarget.gameObject.SetActive(false);
    }
    IEnumerator OpenBarWithSec(bool isBarPointer = false)
    {
        yield return new WaitForSeconds(.5f);
        if (isBarPointer)
            barPointerControl.EnabledObj();
        else
            barControl.EnabledObj();
        ChangeStage(1);
    }
    IEnumerator StartTrajectory()
    {
        yield return new WaitForSeconds(.5f);
        trajectory.gameObject.SetActive(true);
        trajectoryTarget.gameObject.SetActive(true);
        ChangeStage(0);
        tapActive = false;
    }
    void TapBar(bool isBarPointer=false)
    {
        if (isBarPointer)
        {
            barPointerControl.TapScreen(stageId);
        }
        else
            barControl.TapScreen(stageId);
        GiveFeedback();
        tapActive = false;
    }
    void GiveFeedback(bool trajectoryFb = false)
    {
        fbText.gameObject.SetActive(true);
        if (!trajectoryFb)
        {
            fbText.text = barControl.Feedback();
        }
        else
        {
            int rnd = Random.Range(0, 3);
            string newFbText = "";
            switch (rnd)
            {
                case 0:newFbText = "Good!"; break;
                case 1: newFbText = "Yes!"; break;
                case 2: newFbText = "Wow!"; break;
            }
            fbText.text = newFbText;
        }
        fbText.gameObject.GetComponent<Animator>().Play("Feedback", -1, 0f);
    }
    public void ChangeStage(int _stageId) //-1 boþ 0 trajectory 1 kart atma 2 tokat atma 3 enemy sýrasý
    {
        stageId = _stageId;
        if (_stageId != 3) //sýra enemyde deðilse
        {
            tapActive = true;
        }
        if (_stageId == 2) //tokat atmaya geçiyorsa
        {
            HealBars.instance.OpenCloseHealBars(true); //barlarý aç
        }
    }
    public void StartGame()
    {
        CameraController.instance.ChangeCamera(1);
        PaperControl.instance.GivePaper(true);
        ChangeStage(-1);
        StartCoroutine(StartTrajectory());
    }
}
