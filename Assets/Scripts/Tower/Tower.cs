using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Attack Type;
    [SerializeField] private Element Element;
    [SerializeField] private float Damage;
    [SerializeField] private float Cooldown;
    [Space]
    [SerializeField] private Transform GunBarrel;
    [SerializeField] private ParticleSystem ProjectileParticle;
    [SerializeField] private ParticleSystem AOEParticle;

    private List<Enemy> Targets = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_FireLoop());
    }

    private IEnumerator _FireLoop()
    {
        float cooldown = Cooldown;
        while (true)
        {
            //Cooldown
            if (cooldown > 0)
            {
                cooldown -= Time.deltaTime;
                yield return null;
                continue;
            }

            //Fire (hold fire if no targets in range)
            if (Targets.Count > 0)
            {
                //Projectile attack: select the enemy with the lowest rank (so closest to the castle)
                if (Type == Attack.Projectile)
                {
                    int highestRank = -1;
                    Enemy chosenTarget = null;
                    for (int i = Targets.Count-1; i >= 0; i--)
                    {
                        Enemy target = Targets[i];
                        if (target == null)
                        {
                            Targets.RemoveAt(i);
                        }
                        else if (target.Rank > highestRank)
                        {
                            chosenTarget = target;
                            highestRank = target.Rank;
                        }
                    }
                  
                    if(chosenTarget != null)
                    {
                        Fire(chosenTarget);
                        GunBarrel.LookAt(chosenTarget.transform);
                        ProjectileParticle.Play();
                        AudioManager.Instance.PlayRandom("ProjectileTowerShoot");
                    }
                }
                //AOE attack: fire at every enemy in range
                else if (Type == Attack.AreaOfEffect)
                {
                    for (int i = Targets.Count - 1; i >= 0; i--)
                    {
                        Enemy target = Targets[i];
                        if (target == null)
                        {
                            Targets.RemoveAt(i);
                        }
                        else
                        {
                            Fire(target);
                        }
                    }

                    AudioManager.Instance.PlayRandom("AOETowerShoot");
                    AOEParticle.Play();
                }

                cooldown = Cooldown;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Targets.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Targets.Remove(other.GetComponent<Enemy>());
        }
    }

    private void Fire(Enemy target)
    {
        target.TakeDamage(Damage);       
    }

    public Element GetElement()
    {
        return Element;
    }
    public Attack GetAttack()
    {
        return Type;
    }
}

