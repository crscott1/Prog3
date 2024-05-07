using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    // Reference to the agent to reset the episode and apply rewards
    [SerializeField] private Agent agent;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        checkpointSingleList = new List<CheckpointSingle>();
        foreach (Transform checkpointSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public Vector3 toNext(Transform car)
    {
        int carIndex = carTransformList.IndexOf(car);
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carIndex];
        Vector3 posCheck = checkpointSingleList[nextCheckpointSingleIndex].transform.position;
        return posCheck - car.position;
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        int carIndex = carTransformList.IndexOf(carTransform);
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carIndex];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Hide();
            int newCheckpointIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            nextCheckpointSingleIndexList[carIndex] = newCheckpointIndex;
            Debug.Log("Correct");
            agent.AddReward(2.0f); // Reward the agent for correct checkpoint

            // Check if the list is full - i.e., if the agent has returned to the first checkpoint
            if (newCheckpointIndex == 0)
            {
                // Agent has completed a full loop through all checkpoints
                Debug.Log("Lap completed");
                agent.AddReward(10.0f); // Additional reward for completing the circuit
                agent.EndEpisode();  // Reset the episode
            }
        }
        else
        {
            agent.AddReward(-3.0f); // Penalize the agent for wrong checkpoint
        }
    }
}
