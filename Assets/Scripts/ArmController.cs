using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ConfigurableJoint))]
public class ArmController : MonoBehaviour
{
    [Header("Joint")]
    [SerializeField] private ConfigurableJoint joint;

    [Header("Controls")]
    [SerializeField] private Key upKey = Key.W;
    [SerializeField] private Key downKey = Key.S;
    [SerializeField] private Key leftKey = Key.A;
    [SerializeField] private Key rightKey = Key.D;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxAngle = 45f;

    [Header("Drive")]
    [SerializeField] private float spring = 1000f;
    [SerializeField] private float damper = 100f;
    [SerializeField] private float maxForce = 10000f;

    private Quaternion startLocalRotation;
    private float targetX;
    private float targetY;

    private void Awake()
    {
        if (joint == null)
            joint = GetComponent<ConfigurableJoint>();

        // Remember the arm's initial orientation.
        startLocalRotation = transform.localRotation;

        // Use a rotation drive instead of applying torque manually.
        joint.rotationDriveMode = RotationDriveMode.Slerp;

        joint.slerpDrive = new JointDrive
        {
            positionSpring = spring,
            positionDamper = damper,
            maximumForce = maxForce
        };

        // The arm may rotate, but its attachment point stays connected.
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard[leftKey].isPressed) horizontal -= 1f;
        if (keyboard[rightKey].isPressed) horizontal += 1f;
        if (keyboard[downKey].isPressed) vertical -= 1f;
        if (keyboard[upKey].isPressed) vertical += 1f;

        targetY += horizontal * rotationSpeed * Time.deltaTime;
        targetX += vertical * rotationSpeed * Time.deltaTime;

        targetX = Mathf.Clamp(targetX, -maxAngle, maxAngle);
        targetY = Mathf.Clamp(targetY, -maxAngle, maxAngle);
    }

    private void FixedUpdate()
    {
        // Desired orientation relative to the arm's starting pose.
        Quaternion desiredRotation =
            startLocalRotation *
            Quaternion.Euler(targetX, targetY, 0f);

        SetTargetRotationLocal(desiredRotation, startLocalRotation);
    }

    private void SetTargetRotationLocal(
        Quaternion targetLocalRotation,
        Quaternion initialLocalRotation)
    {
        Quaternion jointSpace =
            Quaternion.LookRotation(joint.axis, joint.secondaryAxis);

        Quaternion result = Quaternion.Inverse(jointSpace);

        result *= Quaternion.Inverse(targetLocalRotation)
                  * initialLocalRotation;

        result *= jointSpace;

        joint.targetRotation = result;
    }
}