using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
public class EndScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreKeeper scoreKeeper;
    void Awake()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        
    }

    public void ShowFinalScore(){
        finalScoreText.text = "Congratulations!\nYou got a socre of: " +
                                scoreKeeper.CalculateScore()+"%";
    }
}
