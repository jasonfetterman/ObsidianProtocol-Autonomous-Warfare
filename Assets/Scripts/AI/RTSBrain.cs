using Obsidian.VR;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class RTSBrain : MonoBehaviour
    {
        public float attackRange = 40f;
        public float visionRange = 60f;
        public float fireCooldown = 1.2f;
        public float moveSpeed = 12f;

        public Transform currentTarget;
        public bool holdPosition;

        private float fireTimer;
        private DroneWeaponSystem weapon;
        private Rigidbody rb;

        void Awake()
        {
            weapon = GetComponent<DroneWeaponSystem>();
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            fireTimer += Time.deltaTime;

            AcquireTarget();
            HandleMovement();
            HandleCombat();
        }

        private void AcquireTarget()
        {
            if (currentTarget != null)
            {
                float dist = Vector3.Distance(transform.position, currentTarget.position);
                if (dist > visionRange)
                    currentTarget = null;
            }

            if (currentTarget != null)
                return;

            Collider[] hits = Physics.OverlapSphere(transform.position, visionRange);

            float bestDist = Mathf.Infinity;
            Transform best = null;

            foreach (var hit in hits)
            {
                if (!hit.CompareTag("Unit"))
                    continue;

                if (hit.transform == transform)
                    continue;

                float dist = Vector3.Distance(transform.position, hit.transform.position);

                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = hit.transform;
                }
            }

            currentTarget = best;
        }

        private void HandleMovement()
        {
            if (holdPosition)
                return;

            if (currentTarget == null)
                return;

            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist > attackRange * 0.8f)
            {
                Vector3 dir = (currentTarget.position - transform.position).normalized;
                rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
            }
        }

        private void HandleCombat()
        {
            if (currentTarget == null)
                return;

            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist <= attackRange && fireTimer >= fireCooldown)
            {
                weapon.FireAt(currentTarget.position);  // ✅ FIXED
                fireTimer = 0f;
            }
        }
    }
}
