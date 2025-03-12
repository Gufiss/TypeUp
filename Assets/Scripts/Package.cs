using UnityEngine;

public class Package : MonoBehaviour
{
    /*[HideInInspector] */ public Vector2 endLoc;
    [HideInInspector] public float speed;

    void FixedUpdate()
    {
        // Move towards endLoc at 'speed' per second
        transform.position = Vector2.MoveTowards(transform.position, endLoc, speed * Time.deltaTime);

        // Optional: Destroy or stop movement when reaching the destination
        if ((Vector2)transform.position == endLoc)
        {
            Destroy(gameObject); // Remove package when it arrives
        }
    }
}
