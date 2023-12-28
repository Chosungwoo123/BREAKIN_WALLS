using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    [SerializeField] private RectTransform controlReverseRect;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject dangerText;

    [Space(10)]
    [Header("게임 오버 화면 UI")]
    [SerializeField] private Image gameOverWindow;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI gameOverResultText;
    [SerializeField] private Image gameOverBoundaryLine;
    [SerializeField] private TextMeshProUGUI gameOverBrokenWallText;
    [SerializeField] private TextMeshProUGUI gameOverBrokenWallCountText;
    [SerializeField] private TextMeshProUGUI gameOverCrystalText;
    [SerializeField] private TextMeshProUGUI gameOverCrystalCountText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreCountText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreCountText;
    [SerializeField] private TextMeshProUGUI gameOverRestartText;
    
    #endregion

    #region 게임 관련 변수

    [Space(10)]
    [Header("게임 관련 변수")]
    public float mapMoveSpeed;
    public float scoreMultiply;
    public float controlReverseMinCoolTime;
    public float controlReverseMaxCoolTime;
    public int controlReverseMinTime;
    public int controlReverseMaxTime;
    public GameObject curPlayer;
    
    #endregion

    [HideInInspector] public int breakingWalls = 0;
    [HideInInspector] public int crystalCount = 0;
    [HideInInspector] public int controlMultiply;

    [HideInInspector] public bool isStop;
    
    private float curScore;
    private float curSpeedUpTimer = 0f;
    private float controlReverseTime = 0f;
    private float curControlReverseTimer = 0f;
    private float bestScore = 0;

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
        #region 게임오버 화면 관련 변수 비활성화

        gameOverWindow.gameObject.SetActive(false); 
        gameOverText.gameObject.SetActive(false);
        gameOverResultText.gameObject.SetActive(false);
        gameOverBoundaryLine.gameObject.SetActive(false);
        gameOverBrokenWallText.gameObject.SetActive(false);
        gameOverBrokenWallCountText.gameObject.SetActive(false);
        gameOverCrystalText.gameObject.SetActive(false);
        gameOverCrystalCountText.gameObject.SetActive(false);
        gameOverScoreText.gameObject.SetActive(false);
        gameOverScoreCountText.gameObject.SetActive(false);
        bestScoreText.gameObject.SetActive(false);
        bestScoreCountText.gameObject.SetActive(false);
        gameOverRestartText.gameObject.SetActive(false);
        dangerText.SetActive(false);

        #endregion

        controlReverseTime = Random.Range(controlReverseMinCoolTime, controlReverseMaxCoolTime);
        controlMultiply = 1;
        
        scoreText.text = Mathf.FloorToInt(curScore).ToString();

        StartCoroutine(FadeInObject(fadeImage, 1, 1));

        // 베스트 스코어 불러오기
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetFloat("BestScore");
        }
    }

    private void Update()
    {
        if (isStop)
        {
            return;
        }
        
        curSpeedUpTimer += Time.deltaTime;
        
        //25초 마다 게임 스피드 업
        if (Mathf.FloorToInt(curSpeedUpTimer) >= 25)
        {
            SpeedUp();

            curSpeedUpTimer = 0f;
        }

        curControlReverseTimer += Time.deltaTime;
        
        // 랜덤으로 정해진 시간마다 움직임 반전
        if (Mathf.FloorToInt(curControlReverseTimer) >= controlReverseTime && controlMultiply == 1f)
        {
            // 움직임 반전
            StartCoroutine(ControlReverseRoutine());
        }

        // 움직임 반전까지 3초 남았으면 느낌표 활성화
        if (controlReverseTime - curControlReverseTimer < 3 && controlReverseTime - curControlReverseTimer > 0)
        {
            dangerText.SetActive(true);
        }
        else
        {
            dangerText.SetActive(false);
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

    public void GameOverEvent()
    {
        StartCoroutine(GameOverRoutine());
    }
    
    public void RestartFunction()
    {
        StartCoroutine(RestartRoutine());
    }
    
    private IEnumerator ControlReverseRoutine()
    {
        controlMultiply = -1;
        
        controlReverseRect.anchoredPosition = new Vector2(0, -800);

        controlReverseRect.DOAnchorPosY(440, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(Random.Range(controlReverseMinTime, controlReverseMaxTime));

        controlReverseRect.DOAnchorPosY(800, 0.2f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        controlReverseTime = Random.Range(controlReverseMinCoolTime, controlReverseMaxCoolTime);
        curControlReverseTimer = 0;

        controlMultiply = 1;
    }
    
    private IEnumerator GameOverRoutine()
    {
        isStop = true;
        mapMoveSpeed = 0f;

        // 베스트 스코어 저장
        if (curScore > bestScore)
        {
            bestScore = curScore;

            PlayerPrefs.SetFloat("BestScore", bestScore);
        }
        
        // 게임 오버 텍스트 띄우기
        gameOverWindow.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        gameOverRestartText.gameObject.SetActive(true);
        
        StartCoroutine(FadeOutObject(gameOverText, 1));
        StartCoroutine(FadeOutObject(gameOverRestartText, 1));
        yield return FadeOutObject(gameOverWindow, 1);

        // 결과 텍스트 띄우기
        gameOverResultText.gameObject.SetActive(true);
        gameOverBoundaryLine.gameObject.SetActive(true);
        
        StartCoroutine(FadeOutObject(gameOverResultText, 0.5f));
        yield return FadeOutObject(gameOverBoundaryLine, 0.5f);
        
        // 부순 벽 텍스트 띄우기
        gameOverBrokenWallText.gameObject.SetActive(true);
        yield return FadeOutObject(gameOverBrokenWallText, 0.5f);
        
        gameOverBrokenWallCountText.gameObject.SetActive(true);
        StartCoroutine(TextCountAnimation(gameOverBrokenWallCountText, breakingWalls, 0));
        
        // 크리스탈 텍스트 띄우기
        gameOverCrystalText.gameObject.SetActive(true);
        yield return FadeOutObject(gameOverCrystalText, 0.5f);
        
        gameOverCrystalCountText.gameObject.SetActive(true);
        StartCoroutine(TextCountAnimation(gameOverCrystalCountText, crystalCount, 0));
        
        // 스코어 텍스트 띄우기
        gameOverScoreText.gameObject.SetActive(true);
        yield return FadeOutObject(gameOverScoreText, 0.5f);
        
        gameOverScoreCountText.gameObject.SetActive(true);
        StartCoroutine(TextCountAnimation(gameOverScoreCountText, Mathf.FloorToInt(curScore), 0));

        // 베스트 텍스트 띄우기
        bestScoreText.gameObject.SetActive(true);
        yield return FadeOutObject(bestScoreText, 0.5f);

        bestScoreCountText.gameObject.SetActive(true);
        StartCoroutine(TextCountAnimation(bestScoreCountText, Mathf.FloorToInt(bestScore), 0));
    }
    
    private IEnumerator RestartRoutine()
    {
        yield return StartCoroutine(FadeOutObject(fadeImage, 1, 1));

        SceneManager.LoadScene("MainGame");
    }

    private IEnumerator FadeOutObject(Image _image, float time)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float targetAlpha = _image.color.a;
        float curAlpha = 0;
        float temp = 0;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
        
        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;
            
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }

    private IEnumerator FadeOutObject(Image _image, float time, float fadeAmount)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float targetAlpha = fadeAmount;
        float curAlpha = 0;
        float temp = 0;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
        
        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;
            
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private IEnumerator FadeOutObject(TextMeshProUGUI _text, float time)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float targetAlpha = _text.color.a;
        float curAlpha = 0;
        float temp = 0;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);
        
        while (temp <= time)
        {
            curAlpha += Time.deltaTime * targetAlpha / time;
            
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private IEnumerator FadeInObject(Image _image, float time, float startAmount)
    {
        if (time == 0)
        {
            yield return null;
        }
        
        float curAlpha = startAmount;
        float temp = 0;

        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, startAmount);
        
        while (temp <= time)
        {
            curAlpha -= Time.deltaTime / time;
            
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, curAlpha);
            
            temp += Time.deltaTime;
            
            yield return null;
        }
    }
    
    private IEnumerator TextCountAnimation(TextMeshProUGUI _text, float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            _text.text = ((int)current).ToString();
            yield return null;
        }

        _text.text = Mathf.FloorToInt(target).ToString();
    }
}