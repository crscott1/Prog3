using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class NPCagent_RL : Agent
{
    public NPCCar carController;
    public float throttle;
    public float steering;
    public bool braking;

    public TrackCheckpoints trackCheckpoints;

    private Vector3 startPosition;
    private Vector3 startRotation;

    public override void Initialize()
    {
        carController = GetComponent<NPCCar>();
        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;
    }

    public override void OnEpisodeBegin()
    {
        this.transform.position = startPosition;
        this.transform.eulerAngles = startRotation;

        Rigidbody carRigidbody = GetComponent<Rigidbody>();
        if (carRigidbody != null)
        {
            carRigidbody.velocity = Vector3.zero; 
            carRigidbody.angularVelocity = Vector3.zero;
        }
    }

    

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(carController.transform.localPosition);
        sensor.AddObservation(carController.GetComponent<Rigidbody>().velocity);
        sensor.AddObservation(trackCheckpoints.toNext(transform));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float throttle = actionBuffers.ContinuousActions[0];
        float steering = actionBuffers.ContinuousActions[1];
        bool braking = actionBuffers.ContinuousActions[2] > 0;

        AddReward(-0.001f);
        //Debug.Log($"OnActionReceived called with Throttle: {throttle}, Steering: {steering}, Braking: {braking}");

        carController.steerInput = steering;
        carController.moveInput = throttle;
        carController.brakeInput = braking;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool space = Input.GetKey(KeyCode.Space);

        // Apply the input to the car controller directly
        //carController.SetInputs(horizontal, vertical, space);

        // Optionally, you can also populate the action buffer if you want to see these inputs reflected in the ML-Agents inspector
        continuousActions[0] = vertical;   // moveInput
        continuousActions[1] = horizontal; // steerInput
        continuousActions[2] = space ? 1.0f : 0f; // brakeInput
    }
}
