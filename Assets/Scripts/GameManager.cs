using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 싱글톤

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }

            return instance;
        }
    }
    
    #endregion

    [SerializeField] private CameraShake cameraShake;

    #region UI 관련 변수

    [Space(10)]
    [Header("UI 관련 변수")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform speedUpTextRect;
    [SerializeField] private RectTransform magnetUpTextRect;
    [SerializeField] private Text lifeText;
    
    #endregion

    #region 게임 관련 변수

    [Space(10)]
    [Header("게임 관련 변수")]
    public float mapMoveSpeed;
    public float scoreMultiply;
    public GameObject curPlayer;
    
    #endregion
    
    private float curScore;
    private float curTime = 0f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        scoreText.text = Mathf.FloorToInt(curScore).ToString();
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        
        //25초 마다 게임 스피드 업
        if (Mathf.FloorToInt(curTime) >= 25)
        {
            SpeedUp();

            curTime = 0f;
        }

        ScoreUpdate();
    }

    public void CameraShake(float duration, float magnitube)
    {
        cameraShake.ShakeStart(duration, magnitube);
    }

    public void GetScore(float score)
    {
        StartCoroutine(ScoreCount(curScore + score, curScore));

        curScore += score;
    }

    private IEnumerator ScoreCount(float target, float current)
    {
        float duration = 0.5f;
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            scoreText.text = ((int)current).ToString();
            yield return null;
        }

        current = target;
        scoreText.text = Mathf.FloorToInt(current).ToString();
    }

    private void ScoreUpdate()
    {
        curScore += Time.deltaTime * scoreMultiply;
        
        scoreText.text = Mathf.FloorToInt(curScore).ToString();
    }
    
    private void SpeedUp()
    {
        if (mapMoveSpeed >= 15)
        {
            return;
        }
        
        mapMoveSpeed += 1f;

        speedUpTextRect.anchoredPosition = new Vector2(0, -800);

        speedUpTextRect.DOAnchorPosY(320, 0.3f).SetEase(Ease.OutBack);
        speedUpTextRect.DOAnchorPosY(800, 0.2f).SetDelay(0.7f).SetEase(Ease.InBack);
        
        Debug.Log(mapMoveSpeed);
    }

    public void MagnetUpTextAnimation()
    {
        magnetUpTextRect.anchoredPosition = new Vector2(1400, 200);
        
        magnetUpTextRect.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutBack);
        magnetUpTextRect.DOAnchorPosX(-1400, 0.2f).SetDelay(0.7f).SetEase(Ease.InBack);
    }

    public void SetLifeText(int life)
    {
        lifeText.text = "X " + life.ToString();
    }
}