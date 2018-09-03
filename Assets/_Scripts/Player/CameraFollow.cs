using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class CameraFollow : MonoBehaviour
    {

        GameObject Player;

        // Use this for initialization
        void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = Player.transform.position;
        }
    }
}
