using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HolJackManager : MonoBehaviour
{
   // public Button tryButton;
    public Button EvenButton;
    public Button OddButton;
    public Button resetButton;
    public Button plus100MoneyButton;
    public Button minus100MoneyButton;
    public Button plus500MoneyButton;
    public Button minus500MoneyButton;
    public Button gameStartButton;
    public Text currentMoneyText;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject winPanel;
    public Text winPanelText;
    public GameObject Character;
    public Transform characterTran;
    //private Transform defaultTran;
    public static int currentNum;
    public static int currentMoney = 0;

    public Sprite[] DiceImages;

    public Dice Dice1;
    public Dice Dice2;

    static bool isEven = false;

    public GameObject PinkCup;

    private static HolJackManager _instance;
    public static HolJackManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(HolJackManager)) as HolJackManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }
    void Start()
    {
        //tryButton.onClick.AddListener(() => TryClick());
        EvenButton.onClick.AddListener(() => SelectEven());
        OddButton.onClick.AddListener(() => SelectOdd());
        resetButton.onClick.AddListener(() => ResetClick());
        plus100MoneyButton.onClick.AddListener(() => plusMoney100());
        minus100MoneyButton.onClick.AddListener(() => minusMoney100());
        plus500MoneyButton.onClick.AddListener(() => plusMoney500());
        minus500MoneyButton.onClick.AddListener(() => minusMoney500());
        gameStartButton.onClick.AddListener(() => GameStartClick());
        
    }

    void Update()
    {
        currentMoneyText.text = "현재돈 : \n" + currentMoney.ToString();
    }

    public void GameStartClick()
    {
        if (currentMoney == 0) return;
        Button1.SetActive(true);
        Button2.SetActive(false);
        TryClick();
    }

    public void plusMoney100()
    {
        if (GameManager.Instance.coinMoney < 100) return;
        GameManager.Instance.coinMoney -= 100;
        currentMoney += 100;
    }

    public void minusMoney100()
    {
        if (currentMoney == 0) return;
        GameManager.Instance.coinMoney += 100;
        currentMoney -= 100;
    }

    public void plusMoney500()
    {
        if (GameManager.Instance.coinMoney < 500) return;
        GameManager.Instance.coinMoney -= 500;
        currentMoney += 500;
    }

    public void minusMoney500()
    {
        if (currentMoney < 500) return;
        GameManager.Instance.coinMoney += 500;
        currentMoney -= 500;
    }
    public void TryClick()
    {
       // tryButton.GetComponent<Button>().interactable = false;
        EvenButton.GetComponent<Button>().interactable = true;
        OddButton.GetComponent<Button>().interactable = true;
    }

   
    public void SelectEven()
    {
        isEven = true;
        RollingDice();
    }

    public void SelectOdd()
    {
        isEven = false;
        RollingDice();
    }

    public void RollingDice()
    {
        int Dice1num = Random.Range(0, 6);
        int Dice2num= Random.Range(0, 6);
        Dice1.InsertImage(Dice1num);
        Dice2.InsertImage(Dice2num);
        AddNum(Dice1num, Dice2num);
        EvenButton.GetComponent<Button>().interactable = false;
        OddButton.GetComponent<Button>().interactable = false;
        CharMove();
    }

    public void ResetClick()
    {
        PinkCup.SetActive(true);
        Dice1.ResetImage();
        Dice2.ResetImage();
        resetButton.GetComponent<Button>().interactable = false;
        Button1.SetActive(false);
        Button2.SetActive(true);
        winPanel.SetActive(false);
        Character.transform.position = new Vector3(-5.5f,0.8f, 1f);

    }

    public void AddNum(int Dice1, int Dice2)
    {
        int AddDice ;
        AddDice = Dice1 + Dice2;
        currentNum = AddDice;
        if(AddDice % 2 == 0)
        {
            EvenNum();
        }
        else
        {
            OddNum();
        }
    }

    public void EvenNum()
    {
        Debug.Log("이건 짝수야 ");
        if(isEven ==true)
        {
            Debug.Log("정답 ");

        }
        else
        {
            Debug.Log("오답");
        }

    }

    public void OddNum()
    {
        Debug.Log("이건 홀수야 ");
        if (isEven == true)
        {
            Debug.Log("오답 ");
        }
        else
        {
            Debug.Log("정답");
            
        }

    }

    public void CharMove()
    {
        AudioManager.instance.PlayDice();
        Character.transform.DOMoveX(5.5f, 3f).SetEase(Ease.InSine).OnComplete(()=>ShowWinPanel());
    }

    public void ShowWinPanel()
    {
        PinkCup.SetActive(false);
        if (currentNum % 2 == 0)
        {
            if (isEven == true)
            {
                winPanel.SetActive(true);
                winPanelText.text = "짝수입니다!\n정답입니다. + " + currentMoney * 2 + "코인";
                GameManager.Instance.coinMoney += (currentMoney * 2);
            }
            else
            {
                winPanel.SetActive(true);
                winPanelText.text = "짝수입니다!\n오답입니다. 코인을 잃으셨습니다 ㅋ ";
            }
        }
        else
        {
            if (isEven == true)
            {
                winPanel.SetActive(true);
                winPanelText.text = "홀수입니다!\n오답입니다. 코인을 잃으셨습니다 ㅋ ";
            }
            else
            {
                winPanel.SetActive(true);
                winPanelText.text = "홀수입니다!\n정답입니다. + " + currentMoney * 2 + "코인";
                GameManager.Instance.coinMoney += (currentMoney * 2);
            }
        }
        currentMoney = 0;
        resetButton.GetComponent<Button>().interactable = true;
    }
}
