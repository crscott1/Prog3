using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour {

    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    public bool endEpisodes;

    // Reference to the agent to reset the episode and apply rewards
    [SerializeField] private Agent agent;
    private float maxTimeToReach = 30;
    private float curTimeToReach;

    private float lastReset;
    private void Awake()
    {
        curTimeToReach = maxTimeToReach;
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

    public void ResetCheck()
    {
        foreach (CheckpointSingle single in checkpointSingleList)
        {
            single.Show();
        }

        curTimeToReach = maxTimeToReach;

        for (int i = 0; i < nextCheckpointSingleIndexList.Count; i++)
        {
            nextCheckpointSingleIndexList[i] = 0;
        }

        lastReset = Time.time;
    }

    private void Update()
    {
        curTimeToReach -= Time.deltaTime;
        if (curTimeToReach <= 0 && endEpisodes)
        {
            agent.AddReward(-1);
            agent.EndEpisode();
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
            agent.AddReward(8.0f); // Reward the agent for correct checkpoint
            curTimeToReach = maxTimeToReach;
            NPCagent_RL npc = agent.GetComponent<NPCagent_RL>();
            npc.closest = float.PositiveInfinity;
            // Check if the list is full - i.e., if the agent has returned to the first checkpoint
            if (newCheckpointIndex == 0)
            {
                // Agent has completed a full loop through all checkpoints
                Debug.Log("Lap completed");
                agent.AddReward(15.0f); // Additional reward for completing the circuit
                agent.EndEpisode();  // Reset the episode
            }
        }
        else
        {
            agent.AddReward(-6.0f); // Penalize the agent for wrong checkpoint
            Debug.Log("Wrong");
            agent.EndEpisode();
        }
    }
}
