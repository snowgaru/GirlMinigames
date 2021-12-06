using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    [Header("미니게임매니저들")]
    private BlackjactManager blackManager = null;
    public BlackjackPlayerScript blackDeckManager = null;
    [Header("끝")]

    [SerializeField] private GameObject startUI = null;
    [SerializeField] private GameObject selectUI = null;
    [SerializeField] private GameObject ShopUI = null;
    [SerializeField] private GameObject CardUI = null;
    [SerializeField] private GameObject Blackjack = null;
    [SerializeField] private GameObject twoGame = null;
    [SerializeField] private GameObject Clicker = null;
    [SerializeField] private GameObject Matching = null;
    [SerializeField] private GameObject TutorialUI = null;
    [SerializeField] private GameObject[] tutorial = null;

    [Header("돈 관리")]
    public int coinMoney = 10000;
    public Text[] coinMoneyText;
    
    public void Save()
    {
        PlayerPrefs.SetInt("CoinMoney", coinMoney);
        Debug.Log(PlayerPrefs.GetInt("CoinMoney"));
    }

    public void Load()
    {
        coinMoney = PlayerPrefs.GetInt("CoinMoney");
    }

    void Start()
    {
       // AudioManager.instance.PlayBgm();
        Load();
        Blackjack.GetComponent<CanvasGroup>().alpha = 0;
        Blackjack.GetComponent<CanvasGroup>().interactable = false;
        Blackjack.GetComponent<CanvasGroup>().blocksRaycasts = false;
        Matching.GetComponent<CanvasGroup>().alpha = 0;
        Matching.GetComponent<CanvasGroup>().interactable = false;
        Matching.GetComponent<CanvasGroup>().blocksRaycasts = false;
        tutorial[0].SetActive(true);
    }

    void Update()
    {
        for(int i = 0; i < coinMoneyText.Length; i++)
        coinMoneyText[i].text = "코인 : " + coinMoney;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        else if (_instance != this)
        {
            Destroy(gameObject);
        }

    }

    #region 버튼들
    
    public void OnClickMatching()
    {
        AudioManager.instance.PlayClickSound();
        selectUI.SetActive(false);
        Matching.GetComponent<CanvasGroup>().alpha = 1;
        Matching.GetComponent<CanvasGroup>().interactable = true;
        Matching.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnClickMatchingOut()
    {
        AudioManager.instance.PlayClickSound();
        selectUI.SetActive(true);
        Matching.GetComponent<CanvasGroup>().alpha = 0;
        Matching.GetComponent<CanvasGroup>().interactable = false;
        Matching.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void OnClickTwoGame()
    {
        AudioManager.instance.PlayClickSound();
        twoGame.SetActive(true);
        selectUI.SetActive(false);
        /*
        Blackjack.GetComponent<CanvasGroup>().alpha = 0;
        Blackjack.GetComponent<CanvasGroup>().interactable = false;
        Blackjack.GetComponent<CanvasGroup>().blocksRaycasts = false;
        */
    }

    public void OnClickTwoGameOut()
    {
        AudioManager.instance.PlayClickSound();
        twoGame.SetActive(false);
        selectUI.SetActive(true);

    }
    public void OnClickBlackOut()
    {
        AudioManager.instance.PlayClickSound();
        selectUI.SetActive(true);
        Blackjack.GetComponent<CanvasGroup>().alpha = 0;
        Blackjack.GetComponent<CanvasGroup>().interactable = false;
        Blackjack.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnClickStart()
    {
        AudioManager.instance.PlayClickSound();
        startUI.SetActive(false);
        selectUI.SetActive(true);
    }

    public void OnClickShop()
    {
        AudioManager.instance.PlayClickSound();
        ShopUI.SetActive(true);
        CardUI.SetActive(false);
        selectUI.SetActive(false);
    }

    public void OnClickMainOut()
    {
        AudioManager.instance.PlayClickSound();
        startUI.SetActive(true);
        ShopUI.SetActive(false);
        CardUI.SetActive(false);
        selectUI.SetActive(false);
    }

    public void OnClickCard()
    {
        AudioManager.instance.PlayClickSound();
        ShopUI.SetActive(false);
        CardUI.SetActive(true);
        selectUI.SetActive(false);
    }

    public void OnClickExit()
    {
        AudioManager.instance.PlayClickSound();
        ShopUI.SetActive(false);
        CardUI.SetActive(false);
        selectUI.SetActive(true);
    }

    public void OnClickBlackjack()
    {
        AudioManager.instance.PlayClickSound();
        selectUI.SetActive(false);
        Blackjack.GetComponent<CanvasGroup>().alpha = 1;
        Blackjack.GetComponent<CanvasGroup>().interactable = true;
        Blackjack.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnClickClicker()
    {
        AudioManager.instance.PlayClickSound();
        Clicker.SetActive(true);
        selectUI.SetActive(false);
    }

    public void OnClickClickerOut()
    {
        AudioManager.instance.PlayClickSound();
        Clicker.SetActive(false);
        selectUI.SetActive(true);
    }

    public void OnClickTutorial()
    {
        AudioManager.instance.PlayClickSound();
        startUI.SetActive(false);
        TutorialUI.SetActive(true);
    }

    public void OnClickTutorialOut()
    {
        startUI.SetActive(true);
        TutorialUI.SetActive(false);
    }

    public void TutorialMove(int num)
    {
        AudioManager.instance.PlayClickSound();
        Debug.Log("눌렷음");
        for(int i = 0; i < 16; i++)
        {
            tutorial[i].SetActive(false);
        }
        tutorial[num].SetActive(true);
    }
    #endregion

    public void OnClickGameOut()
    {
        AudioManager.instance.PlayClickSound();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    void OnApplicationQuit()
    {
        Save();
    }
}
