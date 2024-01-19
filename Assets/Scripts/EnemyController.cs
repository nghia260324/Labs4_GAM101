using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{

    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject m_Body;


    [Header("Setting")]
    public int maxHealth = 100;
    public float speed;
    public float targetDistance;
    public int damage = 10;


    private bool m_FacingRight;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private Transform currentPoint;
    public GameObject player;
    public PlayerController m_PlayerController;
    private Vector2 dir;

    private bool target;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        m_PlayerController = player.GetComponent<PlayerController>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = m_Body.GetComponent<Animator>();
        m_FacingRight = false;
        target = false;
        currentPoint = endPoint.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_PlayerController.TakeDamage(damage);
        }
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        dir = player.transform.position - transform.position;
        if (distance < targetDistance)
        {
            target = true;
        }
        else
        {
            target = false;
        }
        if (!target)
        {
            m_Animator.SetBool("isRunning", true);
            if (currentPoint == endPoint.transform)
            {
                m_Rigidbody.velocity = new Vector2(speed, m_Rigidbody.velocity.y);
            } else
            {
                m_Rigidbody.velocity = new Vector2(-speed, m_Rigidbody.velocity.y);
            }
            if ((m_Body.transform.localScale.x < 0 && currentPoint == endPoint.transform) ||
                (m_Body.transform.localScale.x > 0 && currentPoint == startPoint.transform))
            {
                Flip();
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f 
                && currentPoint == endPoint.transform)
            {
                Flip();
                currentPoint = startPoint.transform;
            }
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f
                && currentPoint == startPoint.transform)
            {
                Flip();
                currentPoint = endPoint.transform;
            }
        } else
        {
            m_Animator.SetBool("isRunning", false);
            m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);


            if (dir.x > 0 && m_FacingRight)
            {
                Flip();
            }
            else if (dir.x < 0 && !m_FacingRight)
            {
                Flip();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        m_Animator.SetTrigger("isHit");
/*        m_Rigidbody.AddForce(Vector2.up * 3,ForceMode2D.Impulse);
        if (!m_FacingRight)
        {
            m_Rigidbody.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
        } else
        {
            m_Rigidbody.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }*/
        DamagePopup.Create(transform.position, damage, true);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 localScale = m_Body.transform.localScale;
        localScale.x *= -1;
        m_Body.transform.localScale = localScale;
    }
}
