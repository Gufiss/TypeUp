using UnityEngine;

public class Package_removed : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("DestroyAfterAnimation: No Animator found!");
            return;
        }

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            float animationLength = clipInfo[0].clip.length;
            Destroy(gameObject, animationLength);
        }
        else
        {
            Debug.LogWarning("DestroyAfterAnimation: No animation clip found on layer 0. Destroying object after fallback time.");
            Destroy(gameObject, 2f);
        }
    }
}
