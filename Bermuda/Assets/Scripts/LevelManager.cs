using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public PlayerController Player;
    public GameObject UI;
    public TextMeshProUGUI ScoreText;
    #region Level Settings
    [Header("Level Settings")]
    [Space]
    public int playerSides;
    public float playerSize;
    public int meshCount;
    public float meshSizeMin;
    public float meshSizeMax;
    public int meshSidesMin;
    public int meshSidesMax;
    [Space]
    
    public float meshThickness;
    #endregion

    public GameObject Mesh;
    private void Start()
    {
        List<MeshGenerator> meshes = Mesh.transform.GetComponentsInChildren<MeshGenerator>().ToList();
        Player.MyPolygon.Generate(playerSides, playerSize, meshThickness * 0.3f);
        foreach (var a in meshes)
        {
            a.playerSides = playerSides;
            a.playerSize = playerSize;
            a.meshCount = meshCount;
            a.meshSizeMin = meshSizeMin;
            a.meshSizeMax = meshSizeMax;
            a.meshSidesMin = meshSidesMin;
            a.meshSidesMax = meshSidesMax;
            a.meshThickness = meshThickness;
            a.Generate();
        }
    }

    public int Score;
    public void UpdateScore(int score)
    {
        Score += score;
        ScoreText.text = "Score: " + Score;
    }
    public void ShowUI()
    {
        UI.SetActive(true);
    }
    public void HideUI()
    {
        UI.SetActive(false);
    }
}