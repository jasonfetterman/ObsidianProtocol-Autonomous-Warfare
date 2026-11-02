using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    public sealed class GroundUnitManualDriver : MonoBehaviour, IUnitManualControl
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float turnSpeed = 90f;

        private bool manualControl;
        private Transform controller;
        private Animation unitAnimation;
        private string currentAnimation;
        private float verticalVelocity;
        private const float Gravity = 25f;

        private void Awake()
        {
            unitAnimation = GetComponent<Animation>();
            Debug.Log("[HARLEQUIN ANIM] Driver Awake on " + name +
                "; Animation component=" + (unitAnimation != null));
            if (unitAnimation != null)
                foreach (AnimationState state in unitAnimation)
                    Debug.Log("[HARLEQUIN ANIM] Clip=" + state.name +
                        "; length=" + state.length + "; enabled=" + state.enabled);
            PlayUnitAnimation("Idle");
        }

        public void EnterManualControl(Transform playerController)
        {
            controller = playerController;
            manualControl = true;
            PlayUnitAnimation("Idle");
        }

        public void ExitManualControl()
        {
            manualControl = false;
            controller = null;
            PlayUnitAnimation("Idle");
        }

        private void PlayUnitAnimation(string animationName)
        {
            if (unitAnimation == null ||
                unitAnimation.GetClip(animationName) == null)
                return;

            if (currentAnimation == animationName &&
                unitAnimation.IsPlaying(animationName))
                return;

            if (unitAnimation.Play(animationName))
                currentAnimation = animationName;
        }

        private void MoveOnGround(Vector3 movement)
        {
            transform.position += movement * moveSpeed * Time.deltaTime;

            BoxCollider groundCollider = GetComponent<BoxCollider>();
            if (groundCollider == null)
            {
                Debug.LogError("[HARLEQUIN MOVE] Missing BoxCollider.", this);
                return;
            }

            Bounds bounds = groundCollider.bounds;
            Vector3 rayOrigin = new Vector3(
                bounds.center.x,
                bounds.max.y + 2f,
                bounds.center.z);

            RaycastHit[] hits = Physics.RaycastAll(
                rayOrigin, Vector3.down, 1000f,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

            bool foundGround = false;
            RaycastHit groundHit = default;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.transform == transform ||
                    hit.collider.transform.IsChildOf(transform))
                    continue;

                if (!foundGround || hit.distance < groundHit.distance)
                {
                    groundHit = hit;
                    foundGround = true;
                }
            }

            if (Time.frameCount % 60 == 0)
            {
                Debug.Log("[HARLEQUIN GROUND DIAGNOSTIC] position=" + transform.position +
                    "; colliderBounds=" + bounds +
                    "; foundGround=" + foundGround +
                    "; hit=" + (foundGround ? groundHit.collider.name : "NONE") +
                    "; hitPoint=" + (foundGround ? groundHit.point.ToString() : "NONE") +
                    "; verticalVelocity=" + verticalVelocity, this);
            }
            if (foundGround && verticalVelocity <= 0f)
            {
                SkinnedMeshRenderer visual =
                    GetComponentInChildren<SkinnedMeshRenderer>();

                if (visual != null)
                {
                    float groundCorrection =
                        groundHit.point.y - visual.bounds.min.y;

                    transform.position +=
                        Vector3.up * groundCorrection;
                }
                else
                {
                    float groundCorrection =
                        groundHit.point.y - bounds.min.y;

                    transform.position +=
                        Vector3.up * groundCorrection;
                }

                verticalVelocity = 0f;
            }
            else
            {
                verticalVelocity -= Gravity * Time.deltaTime;
                transform.position += Vector3.up *
                    (verticalVelocity * Time.deltaTime);
            }
        }
        private void Update()
        {
            if (!manualControl)
            {
                PlayUnitAnimation("Idle");
                return;
            }

            float forward = Input.GetAxisRaw("Vertical");
            float sideways = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Debug.Log("[HARLEQUIN INPUT] manual=" + manualControl +
                    "; forward=" + forward + "; sideways=" + sideways +
                    "; driverEnabled=" + enabled + "; objectActive=" + gameObject.activeInHierarchy);

            Vector3 movement =
                (transform.forward * forward +
                 transform.right * sideways).normalized;

            bool moving = forward != 0f || sideways != 0f;
            bool running = moving && Input.GetKey(KeyCode.LeftShift);

            if (moving)
            {
                PlayUnitAnimation(running ? "Running" : "Walking");
                Vector3 beforeMove = transform.position;
                MoveOnGround(movement);
                if (Input.GetKeyDown(KeyCode.W))
                    Debug.Log("[HARLEQUIN MOVE CHECK] before=" + beforeMove +
                        "; after=" + transform.position +
                        "; walkingPlaying=" + (unitAnimation != null && unitAnimation.IsPlaying("Walking")) +
                        "; currentAnimation=" + currentAnimation);
            }
            else
            {
                PlayUnitAnimation("Idle");
            }
        }
    }
}










