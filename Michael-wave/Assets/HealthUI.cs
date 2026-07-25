using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] hearts; // assign left to right in the Inspector
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public void SetHealth(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
    }
}
