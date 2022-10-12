using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyBase
{


    private new void Start()
    {
        SetupDataScriptableObject();
        base.Start();
    }


    public ScriptableObject myData;
    public void SetupDataScriptableObject()
    {


        if (gameObject.name.Contains("Fledgling"))
        {
            var dataTemp = myData as Data_Enemy_Fledgling;
            dropItemName = dataTemp.dataArray[myLevel].Dropitemname;
            try
            {
                curHP = int.Parse(dataTemp.dataArray[myLevel].HP);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                curHP = 1;
            }
            try
            {
                baseDamage = int.Parse(dataTemp.dataArray[myLevel].Normaldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                baseDamage = 1;
            }

            try
            {
                skillDamage = int.Parse(dataTemp.dataArray[myLevel].Skilldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                skillDamage = 1;
            }


            try
            {
                timeToSkillAttack = int.Parse(dataTemp.dataArray[myLevel].Castskilleverysecond);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                timeToSkillAttack = 1;
            }


            try
            {
                attackSpeed = int.Parse(dataTemp.dataArray[myLevel].Attackspeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                attackSpeed = 1;
            }



            try
            {
                moveSpeed = int.Parse(dataTemp.dataArray[myLevel].Movespeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                moveSpeed = 1;
            }

            try
            {
                dropGoldMin = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmin);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMin = 1;
            }

            try
            {
                dropGoldMax = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmax);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMax = 1;
            }




            try
            {
                dropItemChance = int.Parse(dataTemp.dataArray[myLevel].Dropitemchance);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropItemChance = 1;
            }
        }
        if (gameObject.name.Contains("Bunny"))
        {
            var dataTemp = myData as Data_Enemy_Bunny;
            dropItemName = dataTemp.dataArray[myLevel].Dropitemname;
            try
            {
                curHP = int.Parse(dataTemp.dataArray[myLevel].HP);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                curHP = 1;
            }
            try
            {
                baseDamage = int.Parse(dataTemp.dataArray[myLevel].Normaldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                baseDamage = 1;
            }

            try
            {
                skillDamage = int.Parse(dataTemp.dataArray[myLevel].Skilldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                skillDamage = 1;
            }


            try
            {
                timeToSkillAttack = int.Parse(dataTemp.dataArray[myLevel].Castskilleverysecond);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                timeToSkillAttack = 1;
            }


            try
            {
                attackSpeed = int.Parse(dataTemp.dataArray[myLevel].Attackspeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                attackSpeed = 1;
            }



            try
            {
                moveSpeed = int.Parse(dataTemp.dataArray[myLevel].Movespeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                moveSpeed = 1;
            }

            try
            {
                dropGoldMin = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmin);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMin = 1;
            }

            try
            {
                dropGoldMax = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmax);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMax = 1;
            }




            try
            {
                dropItemChance = int.Parse(dataTemp.dataArray[myLevel].Dropitemchance);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropItemChance = 1;
            }
        }
        if (gameObject.name.Contains("MushRoom"))
        {
            var dataTemp = myData as Data_Enemy_MushRoom;
            dropItemName = dataTemp.dataArray[myLevel].Dropitemname;
            try
            {
                curHP = int.Parse(dataTemp.dataArray[myLevel].HP);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                curHP = 1;
            }
            try
            {
                baseDamage = int.Parse(dataTemp.dataArray[myLevel].Normaldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                baseDamage = 1;
            }

            try
            {
                skillDamage = int.Parse(dataTemp.dataArray[myLevel].Skilldamage);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                skillDamage = 1;
            }


            try
            {
                timeToSkillAttack = int.Parse(dataTemp.dataArray[myLevel].Castskilleverysecond);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                timeToSkillAttack = 1;
            }


            try
            {
                attackSpeed = int.Parse(dataTemp.dataArray[myLevel].Attackspeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                attackSpeed = 1;
            }



            try
            {
                moveSpeed = int.Parse(dataTemp.dataArray[myLevel].Movespeed);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                moveSpeed = 1;
            }

            try
            {
                dropGoldMin = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmin);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMin = 1;
            }

            try
            {
                dropGoldMax = int.Parse(dataTemp.dataArray[myLevel].Dropgoldmax);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropGoldMax = 1;
            }




            try
            {
                dropItemChance = int.Parse(dataTemp.dataArray[myLevel].Dropitemchance);
            }
            catch (Exception e)
            {
                Debug.LogError("ParseError: " + e);
                dropItemChance = 1;
            }
        }






        //dropItemPref = null;
    }















}
