using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 기본 스탯
    
    [Space(10)]
    [Header("기본 스탯")]
    [SerializeField] private int health = 3;
    
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float runningAmount = 1.5f;
    
    #endregion

    #region 게임 오브젝트 관련

    [Space(10)]
    [Header("게임 오브젝트 관련")]
    [SerializeField] private Vector2 boundary;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private ParticleSystem magnetEffect;
    [SerializeField] private Magnet magnet;
    
    #endregion

    private float runningMultiply = 1f;
    
    private bool isMoveStop = false;
    private bool isAttacking = false;
    private bool isDashing = false;
    private bool isInvincibility = false;
    
    private Animator anim;
    private WaitForSeconds invincibilityTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        invincibilityTime = new WaitForSeconds(3f);
    }

    private void Update()
    {
        MoveUpdate();
        RunningUpdate();
        AttackUpdate();
    }

    private void MoveUpdate()
    {
        if (isMoveStop)
        {
            return;
        }
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var curPos = transform.position;

        curPos += new Vector3(h, v, 0) * speed * Time.deltaTime * runningMultiply;
        curPos.x = Mathf.Clamp(curPos.x, -boundary.x / 2, boundary.x / 2);
        curPos.y = Mathf.Clamp(curPos.y, -boundary.y / 2, boundary.y / 2);

        transform.position = curPos;
    }

    private void RunningUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            runningMultiply = runningAmount;
        }
        else
        {
            runningMultiply = 1f;
        }
    }

    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(Vector3.zero, boundary);
    }

    private void AttackUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !isDashing)
        {
            anim.SetTrigger("Attack");
        }
    }

    #region 애니메이션 이벤트 함수

    private void MoveStop()
    {
        isMoveStop = true;
    }
    
    private void MovePlay()
    {
        isMoveStop = false;
    }
    
    private void IsAttackingTrue()
    {
        isAttacking = true;
    }
    
    private void IsAttackingFalse()
    {
        isAttacking = false;
    }
    
    private void IsDashingTrue()
    {
        isDashing = true;
    }
    
    private void IsDashingFalse()
    {
        isDashing = false;
    }
    
    #endregion

    public void OnDamage()
    {
        if (isInvincibility)
        {
            return;
        }
        
        GameManager.Instance.CameraShake(0.2f, 5);
        
        //hp 다운
        health--;

        Debug.Log("남은 체력 : " + health);

        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        StartCoroutine(InvincibilityRoutine());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Breaking Wall") && isAttacking)
        {
            col.GetComponent<BreakingWall>().Breaking();
        }

        if (col.CompareTag("Breaking Wall") && !isAttacking)
        {
            OnDamage();
        }

        if (col.CompareTag("Wall"))
        {
            OnDamage();
        }
    
        if (col.CompareTag("Magnet"))
        {
            magnetEffect.Play();
            magnet.scanRange += 0.25f;
            Destroy(col.gameObject);
            GameManager.Instance.MagnetUpTextAnimation();
        }
    }
    
    private IEnumerator InvincibilityRoutine()
    {
        isInvincibility = true;

        yield return invincibilityTime;

        isInvincibility = false;
    }
}