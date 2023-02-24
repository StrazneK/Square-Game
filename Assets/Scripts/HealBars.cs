using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealBars : MonoBehaviour
{
    public Image playerHBar;
    public Image enemyHBar;
    public TextMeshProUGUI playerHealTxt;
    public TextMeshProUGUI enemyHealTxt;
    public static HealBars instance;
    public float healBarYPos;
    bool healBarsOpen = false;
    float playerValue = 1, enemyValue = 1;
    Animator anim;
    bool fillBars = false;
    public float fillSpeed = 1;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (healBarsOpen)
        {
            if (fillBars)
            {
                playerHBar.fillAmount = Mathf.Lerp(playerHBar.fillAmount, playerValue, fillSpeed * Time.deltaTime);
                enemyHBar.fillAmount = Mathf.Lerp(enemyHBar.fillAmount, enemyValue, fillSpeed * Time.deltaTime);
                playerHealTxt.text = (playerHBar.fillAmount * 100).ToString("0");
                enemyHealTxt.text = (enemyHBar.fillAmount * 100).ToString("0");
                if (playerHBar.fillAmount < playerValue + .01f && enemyHBar.fillAmount < enemyValue + .01f)
                {
                    playerHBar.fillAmount = playerValue;
                    enemyHBar.fillAmount = enemyValue;
                    playerHealTxt.text = (playerValue*100).ToString("0");
                    enemyHealTxt.text =  (enemyValue*100).ToString("0");
                    fillBars = false;
                }
            }
        }
    }
    public void OpenCloseHealBars(bool isOpen)
    {
        anim.SetBool("Open", isOpen);
        playerHBar.gameObject.SetActive(isOpen);
        enemyHBar.gameObject.SetActive(isOpen);
        healBarsOpen = isOpen;
    }
    public void SetBarValue(bool isPlayer, float hValue)
    {
        healBarsOpen = true;
        if (isPlayer)
        {
            playerValue = hValue;
        }
        else
        {
            enemyValue = hValue;
        }
        fillBars = true;
    }
    public float GetBarValue(bool isPlayer)
    {
        return isPlayer ? playerValue : enemyValue;
    }
}
