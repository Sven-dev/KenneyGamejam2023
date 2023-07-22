using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public bool GameOver = false;

    [SerializeField] private int TimeBetweenWaves = 120;
    [SerializeField] private float TimeBetweenEnemySpawns = 1f;
    [Space]
    [SerializeField] private List<Enemy> EnemyPrefabs;
    [SerializeField] private Transform EnemySpawnPivot;
    [SerializeField] private Transform EnemyWrapper;
    [Space]
    [SerializeField] private UnityStringEvent OnTimerUpdate;

    private int Wave = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_WaveTimer());
    }

    private IEnumerator _WaveTimer()
    {
        while (!GameOver)
        {
            OnTimerUpdate?.Invoke("Wave in progress");
            AudioManager.Instance.Play("MusicAction");
            AudioManager.Instance.FadeOut("MusicCalm", 1);

            int enemiesToSpawn = Wave * 2 + 18;
            while (enemiesToSpawn > 0)
            {
                //Enemy enemy = Instantiate(EnemyPrefabs[0], EnemySpawnPivot.position, Quaternion.identity, EnemyWrapper);
                //enemy.Rank = enemiesToSpawn;
                enemiesToSpawn--;

                print("spawn enemy");
                yield return new WaitForSeconds(TimeBetweenEnemySpawns);
            }

            Wave++;

            AudioManager.Instance.Play("MusicCalm");
            AudioManager.Instance.FadeOut("MusicAction", 1);

            int cooldown = TimeBetweenWaves;
            while (cooldown > 0)
            {
                OnTimerUpdate?.Invoke(TimeSpan.FromSeconds(cooldown).ToString());
                cooldown--;
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }
}
