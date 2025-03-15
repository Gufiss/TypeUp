using TMPro;
using UnityEngine;

public class Package : MonoBehaviour
{
    [HideInInspector] public Vector2 endLoc;
    [HideInInspector] public float speed;
    [HideInInspector] public string toType;
    [HideInInspector] public Game_Manager manager;

    public TextMeshPro word;

    private void Start()
    {
        word.text = toType;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, endLoc, speed * Time.deltaTime);

        if ((Vector2)transform.position == endLoc)
        {
            manager.activePackages.Remove(gameObject);
            Destroy(gameObject);

            manager.heartManager.LoseLife();
            if(manager.heartManager.lives == 0)
            {
                manager.gameEnd = true;
            }
        }
    }
}
