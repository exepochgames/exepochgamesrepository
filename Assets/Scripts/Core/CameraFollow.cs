using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core{

    // Kamera aktor takibi
    public class CameraFollow : MonoBehaviour
    {
        private Transform playertransform; // Takip edilecek aktorun transformu icin instance referansı

        void Start() // Oyun yüklenmeden once onbellege bilgi aktarımı
        {
            playertransform = GameObject.FindGameObjectWithTag("Player").transform; // "Player" tagına sahip nesnenin Transform componenti tutulur.
        }

        private void Update()
        {
            this.transform.position = playertransform.position; // Bu scriptin bulunduğu nesnenin positionu "Player" taglı nesne ile aynı olur
        }
    }
}