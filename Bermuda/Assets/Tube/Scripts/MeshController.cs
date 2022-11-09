using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    private MeshGenerator meshGenerator;

    public float RotationSpeed;

    private bool rotationSide;
    private void Awake()
    {
        meshGenerator = GetComponent<MeshGenerator>();
    }
    private void OnEnable()
    {
        rotationSide = System.Convert.ToBoolean(Random.Range(0, 2));
        //GameManager.instance.Lose();
    }
    private void Update()
    {
        if(rotationSide)
            transform.Rotate(new Vector3(0, 0, RotationSpeed) * Time.deltaTime);
        else    
            transform.Rotate(new Vector3(0, 0, -RotationSpeed) * Time.deltaTime);
    }
    public void Done()
    {
        gameObject.GetComponent<MeshGenerator>().Invoke("Generate", 4f);
        StartCoroutine(DoneCor());
    }
    IEnumerator DoneCor()
    {
        GameManager.instance.CurrentLevelManager.UpdateScore(Constants.ScoreForOneMesh);
        yield return new WaitForSeconds(1f);
        //meshGenerator.GenerateMesh();
    }
}