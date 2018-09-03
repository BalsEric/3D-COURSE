using System;
using UnityEngine;
using RPG.CameraUI;
using UnityEngine.AI;
using RPG.EnemyCH;
namespace RPG.PlayerCH
{
    [SelectionBase]
    public class Character : MonoBehaviour
    {
        [Header("Movement Properties")]
        [SerializeField] float moveSpeedMultiplier = .7f;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float MovingTurnSpeed = 360;
        [SerializeField] float StationaryMovement = 180;
        [SerializeField] float moveThreshHold = 1f;
      
        [Header("Animator Settings")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Capsule Collider Settings")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 0.85f, 0);
        [SerializeField] float colliderRadius = 0.28f;
        [SerializeField] float colliderHeight = 1.99f;

        [Header("Audio Settings")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("NavMeshAgent Settings")]
        [SerializeField] float navMeshAgentsSteeringSpeed = 1f;
        [SerializeField] float navMeshAgentStopDistance = 1.3f;

       
        NavMeshAgent navMeshAgent;
        Animator animator;
        Rigidbody rigidBody;
        float TurnAmount;
        float ForwardAmount;
        bool isAlive = true;


        private void Awake()
        {
            AddRequiredComponents();
        }
        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            animator.avatar = characterAvatar;
            animator.applyRootMotion = true;
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            rigidBody =gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentsSteeringSpeed;
            navMeshAgent.stoppingDistance = navMeshAgentStopDistance;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = false;
            
            navMeshAgent.acceleration = 8f;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;
            

        }
       
        void Update()
        {
            if(navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
              Move(navMeshAgent.desiredVelocity);
            }
            else
            {
               Move(Vector3.zero);
            }
        }
      
        void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
                // we preserve the existing y part of the current velocity.
                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }

        }
        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }
        void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }
        private void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshHold)
            {
                movement.Normalize();
            }
            var localMove = transform.InverseTransformDirection(movement);
            TurnAmount = Mathf.Atan2(localMove.x, localMove.z);
            ForwardAmount = localMove.z;
        }
        void UpdateAnimator()
        {
            animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", TurnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }
        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(StationaryMovement, MovingTurnSpeed, ForwardAmount);
            transform.Rotate(0, TurnAmount * turnSpeed * Time.deltaTime, 0);
        }
        public void  Kill()
        {
            isAlive = false;
        }
    }
}