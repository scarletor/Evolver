using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flock : MonoBehaviour
{
    // Maximum direction change (degrees) per second
    [SerializeField]
    private float maxDirChangePerSec = 0.1f;

    // Maximum radius for to find flock neighbours
    [SerializeField]
    private float maxFlockRadius = 10f;

    //public Quaternion GetTargetDirection()
    //{
    //    //return Quaternion.RotateTowards(
    //    //    transform.rotation,
    //    //    //GetFlockDirection(),
    //    //    maxDirChangePerSec * Time.deltaTime);
    //}

}
//    private Quaternion GetFlockDirection()
//    {
//        // TODO: Find the flock neighbours - these are all the
//        //       active game objects with the Flock component
//        //       within maxFlockRadius
//        // TODO: If there are no flock neighbours, return the
//        //       current instance's rotation as the flock direction
//        // TODO: Calculate the flock direction by averaging the
//        //       rotation of all the flock neighbours
//        // TODO: Return the calculated flock direction
//    }
//In Unity 3D, a 3D AI flocking behaviour (partial implementation above) implements the flocking behaviour by calculating a general
//    flock direction based on nearby flock members and steering the flock member in the general flock direction.


//Part 1:
//Complete the function Flock.GetFlockDirection() below which calculates the general flock direction.


//Part 2:
//How would you optimize the class implementation to allow for 100's for flocking game object instances?