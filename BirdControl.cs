using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float runSpeed = 20f;
    [SerializeField] float avoidRange = 10f;
    [SerializeField] float allignRange = 10f;
    [SerializeField] int numViewDirections = 300;
    [SerializeField] private float goldernRatio;
    [SerializeField] private float angleIncrement;
    [SerializeField] float sightAngle = 80f;
    [Range(1, 100)]
    [SerializeField] int increment = 1;

    [SerializeField] float allignAngle = 110f;
    [SerializeField] float allignOffest = 30f;
    [SerializeField] float allignSpeed = 2.0f;
    [SerializeField] bool centerSeek = false;
    [SerializeField] float centerAllignSpeed = 1.0f;
    [SerializeField] float centerSeekAngle = 110f;

    float runCoolDown = 0;
    float currentSpeed;

    float angle;
    float phi;

    // Start is called before the first frame update
    void Start()
    {
        goldernRatio = (1 + Mathf.Sqrt(5)) / 2;
        angleIncrement = Mathf.PI * 2 * goldernRatio;
        phi = Mathf.PI * (3 - Mathf.Sqrt(5));
    }

    // Update is called once per frame
    void Update()
    {
        ForwardMotion();
        AvoidCollision();
        AllignDirection();

    }

    private void ForwardMotion()
    {
        runCoolDown -= Time.deltaTime;
        if (runCoolDown > 0)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void AllignDirection()
    {
        GameObject closestBird = null;
        float closestDistance = Mathf.Infinity;
        Vector3 center = transform.position;

        RaycastHit[] nearBys = Physics.SphereCastAll(transform.position, allignRange, Vector3.up, allignRange);


        foreach (RaycastHit near in nearBys)
        {
            if (near.transform.GetComponent<BirdControl>() && near.transform.gameObject != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, near.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBird = near.transform.gameObject;
                }

                center += near.transform.position;
            }
        }





        if (closestBird != null)
        {
            Vector3 heading = closestBird.transform.position - transform.position;
            float closeAngle = Vector3.Angle(heading, transform.forward);

            if (closeAngle < allignAngle)
            {

                Vector3 sheepDirection = closestBird.transform.forward * allignOffest - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, sheepDirection, allignSpeed * Time.deltaTime, 0.00f);
                //Debug.DrawRay(closestBird.transform.position, closestBird.transform.forward * 10, Color.blue);
                //Debug.DrawRay(transform.position, newDirection * 10, Color.blue);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }

        if (centerSeek)
        {
            center = center / nearBys.Length;
        }

        if (nearBys.Length > 1 && centerSeek)
        {
            Vector3 heading = center - transform.position;
            float closeAngle = Vector3.Angle(heading, transform.forward);

            if (closeAngle < centerSeekAngle)
            {
                Vector3 centerDirection = center - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, centerDirection, centerAllignSpeed * Time.deltaTime, 0.00f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                //Debug.DrawRay(transform.position, newDirection * 10, Color.red);
            }
        }
    }

    private void AvoidCollision()
    {
        angle = sightAngle / numViewDirections * goldernRatio;
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;


        for (int i = 0; i < numViewDirections; i += increment)
        {
            Vector3 direction = CalculaterRayDirection(angleIncrement, i);

            //Debug.DrawRay(transform.position, transform.TransformVector(direction) * sightRange, Color.red);
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(transform.position, transform.TransformVector(direction), out hitInfo, avoidRange);

            if (hit && hitInfo.transform.gameObject.GetComponent<PlayerController>())
            {
                runCoolDown = 2f;
                //Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
                //Debug.DrawLine(transform.position, transform.position - hitInfo.point, Color.blue);
                Vector3 targetDirection = transform.position - hitInfo.point;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else if (hit)
            {
                Vector3 targetDirection = transform.position - hitInfo.point;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1f * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }

        }
    }

    private Vector3 CalculaterRayDirection(float angleIncrement, int i)
    {
        float t = (float)i / numViewDirections;
        float inclination = Mathf.Acos(1 - 2 * t) * angle;
        float azimuth = angleIncrement * i * angle;

        float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
        float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
        float z = Mathf.Cos(inclination);
        Vector3 direction = new Vector3(x, y, z);
        return direction;
    }






}
