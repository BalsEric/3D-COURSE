using System;
using UnityEngine;
using RPG.CameraUI;
using UnityEngine.AI;
using RPG.EnemyCH;
namespace RPG.PlayerCH
{
    [RequireComponent(typeof(NavMeshAgent))]
    
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stopDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = .7f;
    
        Vector3 clickPoint;
        NavMeshAgent agent;
      
        Animator animator;
        [SerializeField] float animationSpeedMultiplier = 1.5f;
        [SerializeField] float MovingTurnSpeed = 360;
        [SerializeField] float StationaryMovement = 180;
        [SerializeField] float moveThreshHold = 1f;
        Rigidbody rigidBody;
        float TurnAmount;
        float ForwardAmount;
        
        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraUI.CameraRaycaster>();
          
            
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = true;
            agent.updateRotation = false;
            agent.stoppingDistance = stopDistance;
            animator = GetComponent<Animator>();
            rigidBody = GetComponent<Rigidbody>();

            animator = GetComponent<Animator>();

            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            animator.applyRootMotion = true;

        }
        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
              Move(agent.desiredVelocity);
            }
            else
            {
               Move(Vector3.zero);
            }
        }
        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }    
        }
        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
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
        public void Move(Vector3 movement)
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

        }
    }
}