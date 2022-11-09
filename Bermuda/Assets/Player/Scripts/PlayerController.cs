using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class PlayerController : MonoBehaviour
{
    public float RotationSpeed = 200;
    public float StartSpeed = 10;

    public GameObject BoomParticles;
    public PlayerMeshController MyMeshController;
    public Polygon MyPolygon;

    private float xAxis;

    private PathFollower myPathFollower;
    private void Awake()
    {
        myPathFollower = GetComponent<PathFollower>();
    }
    private void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        MyMeshController.transform.Rotate(new Vector3(0, 0, -1) * RotationSpeed * Time.deltaTime * xAxis);
        myPathFollower.speed = StartSpeed +
            (GameManager.instance.CurrentLevelManager.Score * Constants.SpeedScoreMultiplier);
    }
    public void Lose()
    {
        myPathFollower.enabled = false;
        MyMeshController.gameObject.SetActive(false);
        BoomParticles.SetActive(true);
    }
}