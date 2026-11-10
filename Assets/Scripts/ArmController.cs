using UnityEngine.InputSystem;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    [System.Serializable]
    public class Limb
    {
        [Header("Joint")]
        public ConfigurableJoint joint;

        [Header("Controls")]
        public Key xPositive;
        public Key xNegative;
        public Key zPositive;
        public Key zNegative;

        [Header("Settings")]
        public float rotationSpeed = 90f;
        public float maxAngle = 90f;

        [HideInInspector]
        public Vector3 targetEuler;

        [HideInInspector]
        public Quaternion startLocalRotation;
    }

    [Header("Limbs")]
    public Limb leftArm;
    public Limb rightArm;
    public Limb leftLeg;
    public Limb rightLeg;

    [Header("Joint Drive")]
    public float spring = 500f;
    public float damper = 50f;
    public float maxForce = 1000f;

    private Limb[] limbs;

    private void Awake()
    {
        limbs = new Limb[]
        {
            leftArm,
            rightArm,
            leftLeg,
            rightLeg
        };

        foreach (Limb limb in limbs)
        {
            if (limb.joint == null)
                continue;

            limb.startLocalRotation =
                limb.joint.transform.localRotation;

            limb.targetEuler = Vector3.zero;

            limb.joint.rotationDriveMode =
                RotationDriveMode.Slerp;

            limb.joint.slerpDrive = new JointDrive
            {
                positionSpring = spring,
                positionDamper = damper,
                maximumForce = maxForce
            };
        }
    }

    private void Update()
    {
        foreach (Limb limb in limbs)
        {
            if (limb.joint == null)
                continue;

            float x = 0f;
            float z = 0f;

            if (Keyboard.current == null)
                return;

            if (Keyboard.current[limb.xPositive].isPressed)
                x += 1f;

            if (Keyboard.current[limb.xNegative].isPressed)
                x -= 1f;

            if (Keyboard.current[limb.zPositive].isPressed)
                z += 1f;

            if (Keyboard.current[limb.zNegative].isPressed)
                z -= 1f;

            limb.targetEuler.x +=
                x * limb.rotationSpeed * Time.deltaTime;

            limb.targetEuler.z +=
                z * limb.rotationSpeed * Time.deltaTime;

            limb.targetEuler.x = Mathf.Clamp(
                limb.targetEuler.x,
                -limb.maxAngle,
                limb.maxAngle
            );

            limb.targetEuler.z = Mathf.Clamp(
                limb.targetEuler.z,
                -limb.maxAngle,
                limb.maxAngle
            );
        }
    }

    private void FixedUpdate()
    {
        foreach (Limb limb in limbs)
        {
            if (limb.joint == null)
                continue;

            ApplyRotation(limb);
        }
    }

    private void ApplyRotation(Limb limb)
    {
        Quaternion desiredLocalRotation =
            limb.startLocalRotation *
            Quaternion.Euler(limb.targetEuler);

        Quaternion jointSpace = Quaternion.LookRotation(
            limb.joint.axis,
            limb.joint.secondaryAxis
        );

        limb.joint.targetRotation =
            Quaternion.Inverse(jointSpace) *
            Quaternion.Inverse(desiredLocalRotation) *
            limb.startLocalRotation *
            jointSpace;
    }
}
