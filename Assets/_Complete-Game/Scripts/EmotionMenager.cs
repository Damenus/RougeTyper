using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.WordsRepository;

public enum EmotionEventType
{
    TIME_ELAPSED,
    TYPE_CORRECT_SIGN,
    MISSPELL,
    LOST_HEALTH,
    INCRESE_HEALTH
}

public enum EmotionLevel
{
    MASTER,
    HIGH,
    MEDIUM,
    LOW
}

public class EmotionMenager : MonoBehaviour
{

    static double TIME_ELAPSED_REWARD = -0.0001;
    static double TYPE_CORRECT_SIGN_REWARD = 0.05;
    static double MISSPELL_REWARD = -0.01;

    //static double LOW_LEVEL_SATISFACTION = 0.2;
    static double MEDIUM_LEVEL_SATISFACTION = 0.3;
    static double HIGH_LEVEL_SATISFACTION = 0.6;
    static double MASTER_LEVEL_SATISFACTION = 0.8;


    static EmotionMenager instance = null;
    double Satisfaction { get; set; }

    GameManager gameManager;

    void ChangeSatisfaction(double value)
    {
        if (Satisfaction >= value && (Satisfaction + value <= 1))
            Satisfaction += value;
    }

    public EmotionMenager(GameManager gameManager)
    {
        Satisfaction = 0.5;

        this.gameManager = gameManager;

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

       // DontDestroyOnLoad(gameObject);
    }

    public static EmotionMenager GetInstance()
    {
       // if (instance == null)
         //   instance = new EmotionMenager();
        return instance;
    }

    public void HandleEvent(EmotionEventType even)
    {
        switch (even)
        {
            //liniowo maleje
            case EmotionEventType.TIME_ELAPSED:
                this.ChangeSatisfaction(TIME_ELAPSED_REWARD);
                break;
            case EmotionEventType.TYPE_CORRECT_SIGN:
                this.ChangeSatisfaction(TYPE_CORRECT_SIGN_REWARD);
                LogSatisfaction();
                break;
            case EmotionEventType.MISSPELL:
                this.ChangeSatisfaction(MISSPELL_REWARD);
                LogSatisfaction();
                break;
        }
    }

    public void LogSatisfaction()
    {
        string log = "Satisfaction: " + Satisfaction.ToString();
        Debug.Log(log);
    }

    public EmotionLevel LevelSatisfaction()
    {
        if (Satisfaction < MEDIUM_LEVEL_SATISFACTION)
        {
            return EmotionLevel.LOW;
        }
        else if (Satisfaction < HIGH_LEVEL_SATISFACTION)
        {
            return EmotionLevel.MEDIUM;
        }
        else if (Satisfaction < MASTER_LEVEL_SATISFACTION)
        {
            return EmotionLevel.HIGH;
        }
        else
        {
            return EmotionLevel.MASTER;
        }
    }

    public WordLevel LevelDifficulty()
    {
        EmotionLevel emotionLevel = this.LevelSatisfaction();
        Debug.Log("Level word: " + emotionLevel.ToString());
        switch (emotionLevel)
        {
            case EmotionLevel.MASTER:
                return WordLevel.master;
            case EmotionLevel.HIGH:
                return WordLevel.hard;
            case EmotionLevel.MEDIUM:
                return WordLevel.medium;
            case EmotionLevel.LOW:
                return WordLevel.easy;
            default:
                return WordLevel.easy;
        }

    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isBattle)
            HandleEvent(EmotionEventType.TIME_ELAPSED);
    }
}
