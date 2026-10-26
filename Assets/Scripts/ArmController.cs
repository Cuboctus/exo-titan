
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmController : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody armBody;
    [SerializeField] private float torque = 5f;

    [Header("Movement Keys")]
    [SerializeField] private Key left = Key.A;
    [SerializeField] private Key right = Key.D;
    [SerializeField] private Key up = Key.W;
    [SerializeField] private Key down = Key.S;

    private Vector3 input;

    private void Awake()
    {
        if (armBody == null)
            armBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input = Vector3.zero;

        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard[left].isPressed) input.x -= 1f;
        if (keyboard[right].isPressed) input.x += 1f;
        if (keyboard[down].isPressed) input.y -= 1f;
        if (keyboard[up].isPressed) input.y += 1f;
    }

    private void FixedUpdate()
    {
        if (armBody == null)
            return;

        armBody.AddRelativeTorque(input * torque, ForceMode.Force);
    }
}
