using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI = UnityEngine.UI;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIdx = 0;
    bool hasAnsweredEarly;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    TextMeshProUGUI buttonText;
    UI.Image buttonImage;
    int answerButtonsLength;

    [Header("Timer")]
    [SerializeField] UI.Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;
    public bool isComplete;
    
    private void Awake() {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }
    
    void Start()
    {
        answerButtonsLength = answerButtons.Length;
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update(){
        timerImage.fillAmount = timer.fillFraction;
        if(timer.loadNextQuestion){
            if(progressBar.value == progressBar.maxValue){
                isComplete = true;
                return;
            }
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion){            
            //-1 to default else
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int idx){
        hasAnsweredEarly = true;
        DisplayAnswer(idx);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: "+scoreKeeper.CalculateScore()+"%";
    }

    private void DisplayAnswer(int idx){
        correctAnswerIdx = currentQuestion.GetCorrectAnswerIdx();
        if(idx == correctAnswerIdx){
            questionText.text = "Correct!";
            buttonImage = answerButtons[idx].GetComponent<UI.Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        } 
        else{
            questionText.text = "Sorry, the correct answer was:\n"+currentQuestion.GetAnswer(correctAnswerIdx);
            buttonImage = answerButtons[correctAnswerIdx].GetComponent<UI.Image>();
            buttonImage.sprite = correctAnswerSprite;
        }     
    }

    private void DisplayQuestion(){
        questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answerButtonsLength; i++){
            buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    private void SetButtonState(bool state){
        for(int i=0; i < answerButtonsLength; i++){
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void GetNextQuestion(){
        if(questions.Count > 0){
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion(); 
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    private void GetRandomQuestion(){
        int index = Random.Range(0,questions.Count);
        currentQuestion = questions[index];
        if(questions.Contains(currentQuestion)){
            questions.Remove(currentQuestion);
        }
    }

    private void SetDefaultButtonSprites()
    {
        for(int i=0;i<answerButtonsLength;i++){
            buttonImage = answerButtons[i].GetComponent<UI.Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }    
    }
}
