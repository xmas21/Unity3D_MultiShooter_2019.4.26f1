using UnityEngine;

public class scr_SpawnManager : MonoBehaviour
{
    public static scr_SpawnManager spawnManager;

    [SerializeField]
    private scr_SpawnPoint[] spawnPoints;

    private void Awake()
    {
        spawnManager = this;
        spawnPoints = GetComponentsInChildren<scr_SpawnPoint>();
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)].transform;
    }
}
