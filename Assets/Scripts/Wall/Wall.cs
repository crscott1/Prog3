using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        NPCagent_RL rl = other.collider.GetComponent<NPCagent_RL>();
        if (rl != null)
        {
            //rl.EndEpisode();
        }
    }
}
