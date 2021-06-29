using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject birdPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBird", 2f, 2f);
    }

    // Update is called once per frame
    void SpawnBird()
    {
        Instantiate(birdPrefab, transform.position, transform.rotation);
    }
}
