using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{


    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform playertransform;

        private void Update()
        {
            transform.position = playertransform.position;
        }
    }





}