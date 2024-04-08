using System;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    [SerializeField] private AudioClip pickUpSound;

    public bool isMagnet;

    private void Update()
    {
        // 만약 자석이 활성화 되면 플레이어 쪽으로 이동
        if (isMagnet)
        {
            Vector3 dir = GameManager.Instance.curPlayer.transform.position - transform.position;

            transform.position += dir.normalized * 27 * Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 스코어 업
            GameManager.Instance.crystalCount++;
            GameManager.Instance.GetScore(40);

            // 사운드
            SoundManager.Instance.PlaySound(pickUpSound);

            Instantiate(effect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}