using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public LayerMask enemyLayer;
    public GameObject m_Body;
    public Image healthBar;
    Animator m_Animator;
    Rigidbody2D m_Rigidbody;

    [Header("Setting")]
    public float attackRange = 0.5f;
    public int maxHealth = 100;
    public float speed = 3f;
    public float m_JumpForce;
    public int damageSkill_1;
    public float cooldown_Time_1 = 1f;
    public int damageSkill_2;
    public float cooldown_Time_2 = 1.5f;

    private bool activeSkill = false;
    private bool m_FacingRight;
    private bool m_Grounded;
    private float m_Cooldown_1 = 1f;
    private float m_Cooldown_2 = 1.5f;
    private int currentHealth;
    private int currentSkill;

    private EnemyController targetEnemy;

    public void SetTarget(EnemyController targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    void Start()
    {
        currentHealth = maxHealth;
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = m_Body.GetComponent<Animator>();
        m_FacingRight = true;
    }

    void Update()
    {
        if (m_Rigidbody.velocity.y < -0.7f)
        {
            m_Animator.SetTrigger("isFall");
        }
        float moveInput = moveInput = Input.GetAxisRaw("Horizontal");
        m_Rigidbody.velocity = new Vector2 (moveInput * speed, m_Rigidbody.velocity.y);

        if (moveInput != 0)
        {
            m_Animator.SetBool("isRunning", true);
        } else
        {
            m_Animator.SetBool("isRunning", false);
        }

        if (moveInput > 0 && !m_FacingRight) Flip();
        else if (moveInput < 0 && m_FacingRight) Flip();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Grounded)
            {
                m_Rigidbody.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
                m_Animator.SetTrigger("isJumping");
                m_Grounded = false;
            }
        }

        if (m_Grounded && !activeSkill)
        {
            if (m_Cooldown_1 > cooldown_Time_1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    activeSkill = true;
                    currentSkill = 1;
                    m_Animator.SetTrigger("isPunch");
                    m_Cooldown_1 = 0;
                }
            }
            if (m_Cooldown_2 > cooldown_Time_2)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    activeSkill = true;
                    currentSkill = 2;
                    m_Animator.SetTrigger("isKick");
                    m_Cooldown_2 = 0;
                }
            }
        }
        m_Cooldown_1 += Time.deltaTime;
        m_Cooldown_2 += Time.deltaTime;
    }
    public void Skill_1()
    {
        activeSkill = false;
        if (targetEnemy == null) return;

        int damage = 0;
        switch (currentSkill)
        {
            case 1:
                damage = damageSkill_1;
                break;
            case 2:
                damage = damageSkill_2;
                break;
            default: break;
        }
        targetEnemy.TakeDamage(damage);
    }

    public void Skill_2()
    {
        activeSkill = false;
        if (targetEnemy == null) return;
        int damage = 0;
        switch (currentSkill)
        {
            case 1:
                damage = damageSkill_1;
                break;
            case 2:
                damage = damageSkill_2;
                break;
            default: break;
        }
        targetEnemy.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = (float) currentHealth / (float) maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
            m_Grounded = true;
            m_Animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            SetTarget(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy_2"))
        {
            SetTarget(null);
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
