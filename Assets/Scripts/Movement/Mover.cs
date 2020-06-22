using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Combat;

namespace RPG.Movement{


    public class Mover : MonoBehaviour, IAction
    {
        
        NavMeshAgent navMeshAgent;
        Animator animator;

        private void Start() {
            navMeshAgent=GetComponent<NavMeshAgent>();
            animator=GetComponent<Animator>();
        }

        private void Update()
        {
            UpdateAnimator();

        }

        public void StartMoveAction(Vector3 destination)
        {
           GetComponent<ActionScheduler>().StartAction(this);
           GetComponent<Fighter>().Cancel();
           MoveTo(destination);
        }
 


        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

       

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }



}