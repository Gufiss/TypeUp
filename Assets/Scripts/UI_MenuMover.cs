using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuMover : MonoBehaviour
{
    private RectTransform holder;
    public int direction;
    public float moveSpeed = 5f;
    private Button[] buttons;

    public void TranslateMenu()
    {
        // Disable all button interactions during movement
        SetButtonsInteractable(false);

        Transform parent = transform.parent;
        if (parent != null && parent.parent != null)
        {
            holder = parent.parent.GetComponent<RectTransform>();

            if (holder != null)
            {
                Vector2 targetPos;
                if (direction == 1)
                {
                    targetPos = new Vector2(holder.anchoredPosition.x, -Screen.height + holder.anchoredPosition.y);
                }
                else
                {
                    targetPos = new Vector2(holder.anchoredPosition.x, Screen.height + holder.anchoredPosition.y);
                }

                StartCoroutine(MoveMenu(holder, targetPos));
            }
            else
            {
                Debug.LogError("Holder (Grandparent RectTransform) is NULL!");
            }
        }
        else
        {
            Debug.LogError("Parent or Grandparent is NULL!");
        }
    }

    private IEnumerator MoveMenu(RectTransform menu, Vector2 targetPosition)
    {
        while (Vector2.Distance(menu.anchoredPosition, targetPosition) > 1f)
        {
            menu.anchoredPosition = Vector2.Lerp(menu.anchoredPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        menu.anchoredPosition = targetPosition;


        // Re-enable all buttons after the movement is complete
        SetButtonsInteractable(true);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        // Optionally, refresh the button list if needed
        if (buttons == null || buttons.Length == 0)
        {
            buttons = FindObjectsOfType<Button>();
        }
        foreach (Button btn in buttons)
        {
            btn.interactable = interactable;
        }
    }
}
