using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");

        //transform.Rotate(Vector3.forward * vertical * Time.deltaTime * 40f);
        //transform.Rotate(Vector3.right * horizontal * Time.deltaTime * 40f);

        if(Input.GetKey(KeyCode.Space))
        {   
            transform.Translate(Vector3.up * 10 * Time.deltaTime);
        }
    }
}
