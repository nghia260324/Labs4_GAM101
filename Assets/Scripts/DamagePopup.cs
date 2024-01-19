using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private static int sortingOrder;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    public static DamagePopup Create(Vector3 position, int damage,bool isCriticalHit)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damage,isCriticalHit);
        return damagePopup;
    }


    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damage, bool isCriticalHit)
    {
        textMesh.SetText(damage.ToString());
        if (!isCriticalHit)
        {
            textMesh.fontSize = 2;
            textColor = new Color(0.7f, 0.4f, 0.07f, 1f);
        }
        else
        {
            textMesh.fontSize = 6;
            textColor = new Color(0.7f, 0.1f, 0.06f, 1f);
        }
        textMesh.color = textColor;

        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(1, 1) * 5f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        } else
        {
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
