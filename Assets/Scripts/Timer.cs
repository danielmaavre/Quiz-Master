using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;
    public bool isAnsweringQuestion = false;
    public bool loadNextQuestion;
    public float fillFraction;
    float timerValue; 
    float currentTimer;
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer(){
        
        if(timerValue <= 0 && isAnsweringQuestion){
            timerValue = timeToShowCorrectAnswer;
            currentTimer = timeToShowCorrectAnswer;
            isAnsweringQuestion = false;
            Debug.Log("Reviewing Answer");
        }else if(timerValue <= 0 && !isAnsweringQuestion){
            timerValue = timeToCompleteQuestion;
            currentTimer = timeToCompleteQuestion;
            isAnsweringQuestion = true;
            loadNextQuestion = true;
            Debug.Log("Answering Question");
        }
        fillFraction = timerValue/currentTimer;
        timerValue -= Time.deltaTime;
        Debug.Log("Fill Fraction: "+fillFraction);
    }

    public void CancelTimer(){
        timerValue = 0;
    }
}
