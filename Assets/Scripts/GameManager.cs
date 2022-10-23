using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] Player player;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text highScore;

    // int maxZPos;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);

    TMP_Text gameOverText;

    private void Awake() {
        
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        //belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        // depan
        for (int z = 1; z <= frontDistance; z++)
        {
            var prefab = GetNextRandomTerrainPrefab(z);

            // instantiate bloknya
            CreateTerrain(prefab, z);
        }
        // Debug.Log(Tree.AllPositions.Count);
        // foreach (var treePos in Tree.AllPositions)
        // {
        //     Debug.Log(treePos);
        // }

        player.SetUp(backDistance, extent);
    }

    private int playerLastMaxTravel;

    private void Update()
    {
        Score();
        highScore.text = "HIGH SCORE : " + PlayerPrefs.GetInt("HighScore", 0);
        // cek player masih hidup
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());

        //infinite Terrain System;
        if (player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;

        var posisiBebek = player.transform.position;

        //bikin kedepan - cara 1
        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
        CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance);


        //hapus yang dibelakang
        var lastTB = map[player.MaxTravel - 1 + backDistance];

        // TerrainBlock lastTb = [player.MaxTravel + frontDistance];
        // int lastPos = player.MaxTravel;
        // foreach (var (pos, tb) in map)
        // {
        //     if (pos < lastPos)
        //     {
        //         lastPos = pos;
        //         lastTb = tb;
        //     }
        // }

        //hapus dari daftar
        map.Remove(player.MaxTravel - 1 + backDistance);
        //hilangkan dari scenes
        Destroy(lastTB.gameObject);

        // setup lagi supaya player ga bisa gerak ke belakang
        player.SetUp(player.MaxTravel + backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3);
        gameOverText.text = "YOUR SCORE : " + player.MaxTravel;
        gameOverPanel.SetActive(true);
    }


    // Debug.Log(Tree.AllPositions)

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);
        map.Add(zPos, tb);
        // Debug.Log(map[zPos] is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }

        // penentuan terrain blok dengan probabilitas 50%
        return Random.value > 0.5 ? road : grass;
    }

    public void SceneLoader(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void pauseGame(){
        Time.timeScale = 0;
    }
    public void PlayGame(){
        Time.timeScale = 1;
    }

    public void Score(){
        if(player.MaxTravel > PlayerPrefs.GetInt("HighScore", 0)){
            PlayerPrefs.SetInt("HighScore", player.MaxTravel);
        }
        PlayerPrefs.Save();
    }
}