using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static GeneradorDeOleadas;

public class ReceiveDamage : MonoBehaviour
{
    public int health = 100;
    [HideInInspector] public GeneradorDeOleadas waveManager;
    public TextMeshProUGUI HealthText;
    public Image healthbar;
    public Animator enemyAnimator;
    public NavMeshAgent agent;
    public float totalhealth = 100f;
    public CapsuleCollider hitbox;
    void Start()
    {
        enemyAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        hitbox = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Received Damage");
            Destroy(other.gameObject);
            health -= 10;
            healthbar.fillAmount = health / totalhealth;
            if (health >= 0)
            {
                HealthText.text = health.ToString();
            }
            if (health <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }
    public IEnumerator Death()
    {
        if (waveManager != null)
        {
            waveManager.EnemyDied();
        }
        hitbox.enabled = false;
        agent.speed = 0;
        enemyAnimator.SetBool("IsDead", true);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }
}
