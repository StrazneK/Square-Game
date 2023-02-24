using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Trajectory : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private float distance;
    [SerializeField] private float gravity;
    [SerializeField] private float height;
    [SerializeField] private int segment;

    [HideInInspector] public LaunchData Data;
    private Rigidbody rb;
    private LineRenderer line;

    private void Start()
    {
        Physics.gravity = Vector3.up * gravity;
        rb = GetComponent<Rigidbody>();

        line = GetComponent<LineRenderer>();
        line.positionCount = segment;
        LaunchProjectile();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            LaunchProjectile();
        }
    }

    public void LaunchProjectile()
    {
        line.enabled = true;
          cursor.SetActive(true);

        var target = cursor.transform.position; // transform.position + Vector3.forward * distance; // trajectory sonu
        var origin = transform.position;

        var vo = CalculateVelocity(origin, target).InitialVelocity;
        DrawPath(origin, target);

        rb.velocity = vo;
    }

    public void CloseProjectile()
    {
        //cursor.SetActive(false);
        //line.enabled = false;
    }


    #region Trajectoru formula

    private LaunchData CalculateVelocity(Vector3 origin, Vector3 target)
    {
        float displacementY = target.y - origin.y;
        Vector3 displacementXZ = new Vector3(target.x - origin.x, 0, target.z - origin.z);

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        Data = new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);

        return Data;
    }

    private void DrawPath(Vector3 origin, Vector3 target)
    {
        var launchData = CalculateVelocity(origin, target);
        var previousDrawPoint = transform.position;

        for (int i = 1; i <= segment; i++)
        {
            float simulationTime = i / (float)segment * launchData.TimeToTarget;
            Vector3 displacement = launchData.InitialVelocity * simulationTime + Vector3.up * (gravity * simulationTime * simulationTime) / 2f;
            Vector3 drawPoint = transform.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint;
            line.SetPosition(i - 1, previousDrawPoint);
        }
    }

    public readonly struct LaunchData
    {

        public readonly Vector3 InitialVelocity;
        public readonly float TimeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            InitialVelocity = initialVelocity;
            TimeToTarget = timeToTarget;
        }
    }

    #endregion
}