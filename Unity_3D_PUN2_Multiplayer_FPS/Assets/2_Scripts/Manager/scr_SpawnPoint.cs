using UnityEngine;

public class scr_SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject graphics;

    private void Awake()
    {
        graphics.SetActive(false);
    }

}
