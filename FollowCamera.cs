using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - offset;
    }
}
