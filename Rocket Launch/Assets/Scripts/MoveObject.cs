using UnityEngine;
[DisallowMultipleComponent]
public class MoveObject : MonoBehaviour
{
    [SerializeField] Vector3 movingPosition;
    [SerializeField] float movingSpeed;
    [SerializeField] [Range(0, 1)] float moveProgress;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time * movingSpeed, 1);
        Vector3 offset = movingPosition * moveProgress;
        transform.position = startPosition + offset;
    }
}