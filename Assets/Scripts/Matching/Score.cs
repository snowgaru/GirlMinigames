using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    private int _score;

    public int score
    {
        get => _score;

        set
        {
            if(_score != value) 
                    return;
            _score = value;
        }
    }
    private void Awake() => Instance = this;

}
 