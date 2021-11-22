using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharaterStats))]
public class CharacterCombat : MonoBehaviour
{
    public float attackRate = 1f;
    private float attackCountdown = 0f;
    const float combatCooldown = 5f;
    float lastAttackTime;

    public float attackDelay = .6f;

    public bool IsCombat { get; protected set; }
    public event System.Action OnAttack;

    CharaterStats myStats;
    CharaterStats opponentStats;

    // Start is called before the first frame update
    void Start()
    {
        myStats = GetComponent<CharaterStats>();
    }

    private void Update()
    {
        attackCountdown -= Time.deltaTime;

        if(Time.deltaTime - lastAttackTime > combatCooldown)
        {
            IsCombat = false;
        }
    }
    //Attack target
    public void Attack(CharaterStats targetStats)
    {
        if(attackCountdown <= 0f)
        {
            //attack target after certain time
            StartCoroutine(DoDamage(targetStats, attackDelay));
            //opponentStats = targetStats;
            if (OnAttack != null)
                OnAttack();

            attackCountdown = 1f / attackRate;
            IsCombat = true;
            lastAttackTime = Time.time;
        }
        
    }

    IEnumerator DoDamage(CharaterStats stats, float delay)
    {
        yield return new WaitForSeconds(delay);

        stats.TakeDamage(myStats.damage.GetValue());
        if (stats.currentHealth <= 0)
            IsCombat = false;
    }

    public void AttackHit_AnimationEvent()
    {
        opponentStats.TakeDamage(myStats.damage.GetValue());
        if (opponentStats.currentHealth <= 0)
            IsCombat = false;
    }
}
