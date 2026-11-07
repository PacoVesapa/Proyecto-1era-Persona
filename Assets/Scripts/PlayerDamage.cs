using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public static bool jugadorMuerto = false;
    public int vida = 100;
    public Image BarraVida;
    public Image BarraVidaFondo;
    public float totalhealth = 100f;
    public float tiempoinmune = 2f;
    public bool esInmune = false;
    public float healingcd = 10f;
    public float tiempoSinRecibirDanio = 0f;
    public GameObject panelDerrota;
    private Coroutine curacion;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (jugadorMuerto)
        {
            return;
        }
        if (vida < totalhealth)
        {
            tiempoSinRecibirDanio += Time.deltaTime;
            if (!esInmune && tiempoSinRecibirDanio >= healingcd && curacion == null)
            {
                curacion = StartCoroutine(CurarVida());
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (jugadorMuerto)
        {
            return;
        }
        if (hit.gameObject.CompareTag("Enemy") && !esInmune)
        {
            Debug.Log("Received Damage");
            StartCoroutine(RecibirDanio(10));
        }
        else if (hit.gameObject.CompareTag("BigEnemy") && !esInmune)
        {
            Debug.Log("Received Damage");
            StartCoroutine(RecibirDanio(30));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (jugadorMuerto)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy") && !esInmune)
        {
            Debug.Log("Received Damage");
            StartCoroutine(RecibirDanio(10));
        }
        else if (other.gameObject.CompareTag("BigEnemy") && !esInmune)
        {
            Debug.Log("Received Damage");
            StartCoroutine(RecibirDanio(30));
        }
    }
    public IEnumerator RecibirDanio(float daño)
    {
        if (jugadorMuerto)
        {
            yield break;
        }
        tiempoSinRecibirDanio = 0f;
        if (curacion != null)
        {
            StopCoroutine(curacion);
            curacion = null;
        }
        vida -= (int)daño;
        if (vida <= 0)
        {
            vida = 0;
            BarraVida.fillAmount = 0f;
            Die();
            yield break;
        }
        BarraVida.fillAmount = vida / totalhealth;
        esInmune = true;
        yield return new WaitForSeconds(tiempoinmune);
        yield return StartCoroutine(FondoVida());
        esInmune = false;
        yield return null;
    }
    public IEnumerator CurarVida()
    {
        while (vida < totalhealth)
        {
            BarraVida.fillAmount += Time.deltaTime;
            BarraVida.fillAmount = Mathf.Clamp01(BarraVida.fillAmount);
            vida = Mathf.RoundToInt(BarraVida.fillAmount * totalhealth);
            BarraVidaFondo.fillAmount = BarraVida.fillAmount;
            yield return null;
        }
        curacion = null;
    }
    public IEnumerator FondoVida()  
    {
        while (BarraVidaFondo.fillAmount > BarraVida.fillAmount)
        {
            BarraVidaFondo.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void Die()
    {
        if (jugadorMuerto)
        {
            return;
        }
        Debug.Log("Player has died");
        jugadorMuerto = true;
        StarterAssets.StarterAssetsInputs.jugadorMuerto = true;
        StopAllCoroutines();
        curacion = null;
        panelDerrota.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}
