using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level")]
public class enemyData : ScriptableObject
{
    public Vector3 EnemyPos;
    public List<GameObject> enemiesInScene;
    public int enemyCountInMap;

        
    
    



    public void Awake()
    {
       collectEnemiesInMap();
        

    }
    public void Update()
    {

    }


    private void collectEnemiesInMap()
    {
        enemiesInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemy"));
        
        foreach ( GameObject enemy in enemiesInScene)
        {
            
            enemyCountInMap++;
            Debug.Log( enemyCountInMap + " enemy logged");
        }
       
    }



}
