using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEditor;

public class GroundLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetupSelf();
    }

    public Data_Level_1 DataLv1;
    public Data_Level_2 DataLv2;
    public void ScaleAll()
    {
        //shadows.ForEach(sprite => {
        //    sprite.gameObject.transform.DOScaleY(UnityEngine.Random.Range(1,5), 1);
        //})
        gameObject.transform.DOScaleY(UnityEngine.Random.Range(1, 5), 1);

    }

    string creaturePath = "Creature/";
    float offsetSpawn2Creature = 2.5f;
    public void SetupSelf()
    {

        if (DataLv1)
        {
            List<Data_Level_1Data> list = new List<Data_Level_1Data>(DataLv1.dataArray);

            list.ForEach(ground =>
            {
                if (ground.Enemy1_Name != "")
                {
                    var curGround = transform.Find(ground.Groundname);
                    var newMonsterLoad = Resources.Load(creaturePath + ground.Enemy1_Name) as GameObject;
                    var newMonster = Instantiate(newMonsterLoad);
                    newMonster.transform.position = new Vector3(curGround.transform.position.x + UnityEngine.Random.Range(-offsetSpawn2Creature, offsetSpawn2Creature), 0.22f, curGround.transform.position.z + UnityEngine.Random.Range(-offsetSpawn2Creature, offsetSpawn2Creature));
                    newMonster.transform.SetParent(curGround.transform);

                    newMonster.GetComponent<EnemyBase>().myLevel =int.Parse( ground.Enemy1_Level);
                }

                if (ground.Enemy2_Name != "")
                {
                    var curGround = transform.Find(ground.Groundname);
                    var newMonsterLoad = Resources.Load(creaturePath + ground.Enemy2_Name) as GameObject;
                    var newMonster = Instantiate(newMonsterLoad);
                    newMonster.transform.position = new Vector3(curGround.transform.position.x + UnityEngine.Random.Range(-offsetSpawn2Creature, offsetSpawn2Creature), 0.22f, curGround.transform.position.z + UnityEngine.Random.Range(-offsetSpawn2Creature, offsetSpawn2Creature));
                    newMonster.transform.SetParent(curGround.transform);
                    newMonster.GetComponent<EnemyBase>().myLevel = int.Parse(ground.Enemy2_Level);

                }


            });




            return;

        }

    }



    public int level;






}
