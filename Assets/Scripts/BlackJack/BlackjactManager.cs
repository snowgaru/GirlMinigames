using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackjactManager : MonoBehaviour
{
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;

    public Button outBtn;

    public Text standBtnText;
    public Text scoreText;
    public Text dealerScoreText;
    public Text cashText2;
    public Text mainText;
    public GameObject mainTextPanel;

    public GameObject hideCard;

    public GameObject playerCards;

    private int standClicks = 0;
    public int pot = 0;
    public static int winCounter = 0;
    public static int currentMoney = 0;

    public BlackjackPlayerScript playerScript;
    public BlackjackPlayerScript dealerScript;

    void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
    }

    public void Out()
    {
        playerCards.transform.DOMoveY(30f, 0.5f);
        mainTextPanel.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        Debug.Log(GameManager.Instance.coinMoney);
        Debug.Log(playerScript.money);
        GameManager.Instance.coinMoney += playerScript.money;
        Debug.Log(GameManager.Instance.coinMoney);
        playerScript.money = 0;
        winCounter = 0;
        currentMoney = 0;
    }

    public void FirstStartBlack()
    {
        playerScript.money = 0;
        cashText2.text = "현재 받는 코인: \n0";
        scoreText.text = "Hand: ";
    }
    public void DealClicked()
    {
        AudioManager.instance.PlayCardSound();
        outBtn.interactable = false;
        playerCards.transform.DOMoveY(0f, 0.5f);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        playerScript.ResetHand();
        dealerScript.ResetHand();
        mainTextPanel.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        hideCard.GetComponent<Renderer>().enabled = true;
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "스탠드";
        if(winCounter == 0)
        {
            UpdateCoinPanel();
        }
    }

    public void UpdateCoinPanel()
    {
        cashText2.text = "현재 받는 코인:\n" + playerScript.GetMoney().ToString();
    }

    private void HitClicked()
    {
        AudioManager.instance.PlayCardSound();
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        AudioManager.instance.PlayClickSound();
        standClicks++;
        if(standClicks > 1)
        {
            RoundOver();
        }
        HitDealer();
        standBtnText.text = "콜";
    }

    private void HitDealer()
    {
        while(dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: "+ dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }

    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21; ///여기바꿧으므ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 21   21
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        if(playerBust && dealerBust)
        {
            mainText.text = "모두 21을 넘었습니다. 돈이 다음판까지 보존됩니다.";
            //playerScript.AdjustMoney(pot / 2);
        }
        else if(playerBust || dealerScript.handValue > playerScript.handValue && dealerScript.handValue < 22)
        {
            mainText.text = "딜러가 이겼습니다.";
            winCounter = 0;
            currentMoney = 0;
            playerScript.money = 0;
        }
        else if(dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "당신이 이겼습니다.";
            playerScript.AdjustMoney( currentMoney + 100 + (100*winCounter)); /// 이겼을때 들어가는 돈의 공식!
            currentMoney = 100 + (100 * winCounter);
            playerScript.GetMoney();
            UpdateCoinPanel();
            winCounter++;
            Debug.Log("승리. 윈카운터 증가 현재 윈카운터 값: " + winCounter);
        }
        else if(playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "동점입니다.";
            //playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        if(roundOver)
        {
            //playerScript.money = 0;
            outBtn.interactable = true;
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainTextPanel.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            //cashText.text = playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }
    
    /*
    void BetClicked()
    {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1));
        playerScript.AdjustMoney(-intBet);
        //cashText.text = playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = pot.ToString();
    }
    */
}
