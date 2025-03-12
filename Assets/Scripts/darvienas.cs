using UnityEngine;
using UnityEngine.Tilemaps;

public class darvienas : MonoBehaviour
{
    private Tilemap tilemap;
    public float animationSpeed = 1f;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        UpdateAnimationSpeed();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animationSpeed += 1f;
            UpdateAnimationSpeed();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animationSpeed = Mathf.Max(0.1f, animationSpeed - 1f);
            UpdateAnimationSpeed();
        }
    }

    void UpdateAnimationSpeed()
    {
        tilemap.animationFrameRate = 1f * animationSpeed;
        Debug.Log("New Animation Speed: " + tilemap.animationFrameRate);
    }
}
