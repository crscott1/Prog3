using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour {

    private TrackCheckpoints trackCheckpoints;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        Show();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "NPC Car")
        {
            trackCheckpoints.CarThroughCheckpoint(this, other.transform);
        }
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints) {
        this.trackCheckpoints = trackCheckpoints;
    }

    public void Show() {
        meshRenderer.enabled = true;
    }

    public void Hide() {
        meshRenderer.enabled = false;
    }

}
