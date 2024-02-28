using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "New Level", menuName = "Game/Level")]
public class enemyData : ScriptableObject
{
    public Vector3 EnemyPos;
    public List<GameObject> enemiesInScene;

        
    




    public void Awake()
    {
        enemiesInScene = new List<GameObject>();

    }
    

    private void collectEnemiesInMap()
    {

    }



}
