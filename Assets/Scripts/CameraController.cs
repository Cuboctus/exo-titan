using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followed;
    private Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - followed.transform.position; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followed.transform.position + offset; 
    }
}
