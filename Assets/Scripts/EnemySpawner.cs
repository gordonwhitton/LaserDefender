using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingwave = 0;
    [SerializeField] bool looping = false;

    // Start is called before the first frame update
    IEnumerator Start() //changing from void Start, make a co-routine
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        Debug.Log("waveConfigs count:" + waveConfigs.Count);

        for(int waveIndex = startingwave; waveIndex < waveConfigs.Count; waveIndex++ )
        {
            Debug.Log("waveIndex:" + waveIndex);

            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        //Debug.Log(waveConfig.GetWaypoints()[0].transform.position);
        //Debug.Log(waveConfig.GetTimeBetweenSpawns());

        Debug.Log("number of enemies :" + waveConfig.GetNumberOfEnemies());

        Debug.Log("GetWaypoints :" + waveConfig.GetWaypoints().Count);

        for (int i = 0; i < waveConfig.GetNumberOfEnemies(); i++)
        {

            var newEnemy =  Instantiate(
           waveConfig.GetEnemyPrefab(),
           waveConfig.GetWaypoints()[0].transform.position,
           Quaternion.identity); //Quaternion.identity means just use rotation started with

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
