using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement{


    public class Mover : MonoBehaviour, IAction
    {
        ///----------------------------------------------------------| CLASS INSTANCEs
        NavMeshAgent navMeshAgent; // Nav mesh componenti icin referans
        Animator animator; // Animantor componenti icin ref
        Health health;

        ///----------------------------------------------------------| UNITY METHODs
        private void Start() {
            navMeshAgent=GetComponent<NavMeshAgent>(); // NavMesh Initializer
            animator=GetComponent<Animator>();  // Animator Init
            health = GetComponent<Health>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator(); 
        }

        ///----------------------------------------------------------| MAIN METHODs
        public void StartMoveAction(Vector3 destination) 
        {
           GetComponent<ActionScheduler>().StartAction(this); // Aktif islem kontrolcüsüne yeni islem talebi
           GetComponent<Fighter>().Cancel();  // Saldırı islemi iptal edilir
           MoveTo(destination); // Parametre olarak, PlayerContollerda alınan mouse raycastın carptıgı noktanın positionu gönderilir
        }

        public void MoveTo(Vector3 destination) 
        {
            navMeshAgent.destination = destination; // Navmesh sistemi icin hedef pozisyonu olarak gelen parametre gonderimi
            navMeshAgent.isStopped = false;  // Durma eyleminden hareket eylemine geçmenin kesinlestirilmesi (Bug fixlemek için)
        }

        public void Cancel() 
        {
            navMeshAgent.isStopped = true; // isStopped fonksiyonuyla navMesh üzerinden hareket durdurulur.
        }


        ///----------------------------------------------------------| ANIMATOR EVENTs
        private void UpdateAnimator() 
        {
            Vector3 velocity = navMeshAgent.velocity;  // Hareketin ivmesinin navMesh sisteminden çekilmesi (World eksenine göre)
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // Local ivmenin çekilmesi (Karakterin Transform degerlerine gore (Animasyon hız/ivme orantısı bug fix))
            float speed = localVelocity.z; // Aktorün hızı kendi Transformu üzerinden hesaplanan ivmenin z eksenindeki degeri kadardır(z yönünde 5f ise hızı 5f per second)
            animator.SetFloat("forwardSpeed", speed); // Hız değeri animatorde 0 iken sabit durma animasyonunu oynatır ,bu deger arttıkca kosma animasyonuna gecis saglanır
        }
    }
}