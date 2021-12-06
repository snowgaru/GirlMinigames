using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TwoManager : MonoBehaviour
{
    CardManager cardManager;
    public GameObject[] n;
    public GameObject Quit;
    public GameObject image;
    public Text Score, BestScore, Plus;
    public int gameScore;

    public Button BackButton;
    bool wait, move, stop;
    public Button newGame;
    int x, y, i, j, k, l, score;
    Vector3 firstPos, gap;
    GameObject[,] Square = new GameObject[4, 4];

    void Start()
    {
        
    }

    public void GameStart()
    {
        AudioManager.instance.PlayClickSound();
        newGame.interactable = false;
        stop = false;
        l = 0;
        Spawn();
        Spawn();
        BackButton.interactable = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

        if (stop) return;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            wait = true;
            firstPos = Input.GetMouseButtonDown(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position;
        }

        if (Input.GetMouseButton(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            gap = (Input.GetMouseButton(0) ? Input.mousePosition : (Vector3)Input.GetTouch(0).position) - firstPos;
            if (gap.magnitude < 100) return;
            gap.Normalize();

            if (wait)
            {
                wait = false;
                if (gap.y > 0 && gap.x > -0.5f && gap.x < 0.5f) for (x = 0; x <= 3; x++) for (y = 0; y <= 2; y++) for (i = 3; i >= y + 1; i--) MoveOrCombine(x, i - 1, x, i);
                else if (gap.y < 0 && gap.x > -0.5f && gap.x < 0.5f) for (x = 0; x <= 3; x++) for (y = 3; y >= 1; y--) for (i = 0; i <= y - 1; i++) MoveOrCombine(x, i + 1, x, i);
                else if (gap.x > 0 && gap.y > -0.5f && gap.y < 0.5f) for (y = 0; y <= 3; y++) for (x = 0; x <= 2; x++) for (i = 3; i >= x + 1; i--) MoveOrCombine(i - 1, y, i, y);
                else if (gap.x < 0 && gap.y > -0.5f && gap.y < 0.5f) for (y = 0; y <= 3; y++) for (x = 3; x >= 1; x--) for (i = 0; i <= x - 1; i++) MoveOrCombine(i + 1, y, i, y);
                else return;

                if (move)
                {
                    move = false;
                    Spawn();
                    k = 0;
                    l = 0;

                    if (score > 0)
                    {
                        Plus.text = "+" + score.ToString() + "    ";
                        Plus.GetComponent<Animator>().SetTrigger("PlusBack");
                        Plus.GetComponent<Animator>().SetTrigger("Plus");
                        Score.text = (int.Parse(Score.text) + score).ToString();
                        gameScore += score;
                        score = 0;
                    }

                    for (x = 0; x <= 3; x++) for (y = 0; y <= 3; y++)
                        {
                            if (Square[x, y] == null) 
                            { 
                                k++;
                                continue; 
                            }
                            if (Square[x, y].tag == "Combine") 
                                Square[x, y].tag = "Untagged";
                        }
                    if (k == 0)
                    {
                        for (y = 0; y <= 3; y++) for (x = 0; x <= 2; x++) if (Square[x, y].name == Square[x + 1, y].name) l++;
                        for (x = 0; x <= 3; x++) for (y = 0; y <= 2; y++) if (Square[x, y].name == Square[x, y + 1].name) l++;
                        if (l == 0) 
                        { 
                            stop = true; 
                            Quit.SetActive(true); 
                            //return; 
                        }
                    }
                }
            }
        }
    }

    public void ClearN()
    {
        AudioManager.instance.PlayClickSound();
        for (int a = 0; a < 4; a ++) for(int b = 0; b < 4; b++)
            {
                Square[a, b] = null;
            }
        GameManager.Instance.coinMoney += gameScore;
        Debug.Log(gameScore);
        gameScore = 0;
        Score.text = 0.ToString();
        Quit.SetActive(false);
        newGame.interactable = true;
        BackButton.interactable = true;
    }

    void MoveOrCombine(int x1, int y1, int x2, int y2)
    {
        if (Square[x2, y2] == null && Square[x1, y1] != null)
        {
            move = true;
            Debug.Log(GetComponent<TwoMoving>());
            Square[x1, y1].GetComponent<TwoMoving>().Move(x2, y2, false);
            Square[x2, y2] = Square[x1, y1];
            Square[x1, y1] = null;
        }

        if (Square[x1, y1] != null && Square[x2, y2] != null && Square[x1, y1].name == Square[x2, y2].name && Square[x1, y1].tag != "Combine" && Square[x2, y2].tag != "Combine")
        {
            move = true;
            for (j = 0; j <= 16; j++) if (Square[x2, y2].name == n[j].name + "(Clone)") 
                    break;
            Square[x1, y1].GetComponent<TwoMoving>().Move(x2, y2, true);
            Destroy(Square[x2, y2]);
            Square[x1, y1] = null;
            Square[x2, y2] = Instantiate(n[j + 1], new Vector3(1.2f * x2 - 1.8f, 1.2f * y2 - 1.8f, 0), Quaternion.identity);
            Square[x2, y2].transform.parent = image.transform;
            Square[x2, y2].tag = "Combine";
            Square[x2, y2].GetComponent<Animator>().SetTrigger("Combine");
            score += (int)Mathf.Pow(2, j + 2);
        }
    }

    void Spawn()
    {
        while (true) 
        { 
            x = Random.Range(0, 4); 
            y = Random.Range(0, 4); 
            if (Square[x, y] == null) 
                break; 
        }
        Square[x, y] = Instantiate(Random.Range(0, int.Parse(Score.text) > 800 ? 4 : 8) > 0 ? n[0] : n[1], new Vector3(1.2f * x - 1.8f, 1.2f * y - 1.8f, 0), Quaternion.identity);
        Square[x, y].transform.parent = image.transform;
        Square[x, y].GetComponent<Animator>().SetTrigger("Spawn");
    }

}


