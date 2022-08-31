using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class PetController : CreatureBase
{
    // Start is called before the first frame update
    void Start()
    {
        baseName = gameObject.name;

    }
    string baseName;
    public bool isOwned;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOwned == false) return;
        FollowPlayer();
        AttackEnemy();
        PetAttack();
    }


    public PlayerController playerControl;
    public GameObject _petTarget;
    public void AttackEnemy()
    {
        if (playerControl._targetRange == null) return;


        _petTarget = playerControl._targetRange;



    }
    public float distanceFollow, speedLookAt;

    public GameObject player;

    public float distanceDisplay;
    public bool canFollowPlayer;

    public override void FollowPlayer()
    {
        distanceDisplay = Vector3.Distance(player.transform.position, gameObject.transform.position);




        if (Vector3.Distance(player.transform.position, gameObject.transform.position) > distanceFollow)
        {
            canFollowPlayer = true;
        }

        if (canFollowPlayer)
        {
            if (_petTarget == null)
                MoveTo(player);
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 1)
            {
                canFollowPlayer = false;
                ChangeState(PetStateEnum.idle);
            }

        }
    }
    public PetStateEnum petState;
    public Animator _animation;
    public void ChangeState(PetStateEnum state)
    {
        switch (state)
        {
            case PetStateEnum.move:
                _animation.SetTrigger("Move");
                break;
            case PetStateEnum.idle:
                _animation.SetTrigger("Idle");

                break;
            case PetStateEnum.attack:
                _animation.SetTrigger("petAttack");
                _animation.SetBool("hasTarget", true);

                break;
        }

        petState = state;
        gameObject.name = baseName + "_" + state;

    }
    public float rangeStopMoveToEnemy;

    public void PetAttack()
    {
        if (_petTarget == null) return;


        if (Vector3.Distance(_petTarget.transform.position, gameObject.transform.position) < rangeStopMoveToEnemy)
        {
            ChangeState(PetStateEnum.attack);
            Debug.LogError("111");
        }
        else
        {
            MoveTo(_petTarget);
            Debug.LogError("22");

        }

    }


    public void MoveTo(GameObject target)
    {
        Debug.LogError("PET MOVE");
        ChangeState(PetStateEnum.move);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        //slowly lookat
        Vector3 relativePos = target.transform.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speedLookAt * Time.deltaTime);
    }


    [Button]
    public void FinishAttackPet()
    {

        if (playerControl._targetRange == null)
        {
            _animation.SetBool("hasTarget", false);
            return;
        }
        else
        {
            _animation.SetBool("hasTarget", true);
        }
        if (petState == PetStateEnum.move) return;
        ChangeState(PetStateEnum.attack);

    }




    [Button]
    public void test()
    {
        _animation.SetTrigger("petAttack");

    }





    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("TOYUDHJ");
        isOwned = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("TOYUDHJ");

    }




    public enum PetStateEnum
    {
        move,
        idle,
        attack

    }
}