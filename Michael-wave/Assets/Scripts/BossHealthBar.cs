using UnityEngine;

// The boss is spawned at runtime by WaveSpawner, so this can't hold a scene reference to it.
// It looks the boss up once it appears, and hides the bar whenever there isn't one.
public class BossHealthBar : MonoBehaviour
{
    public RectTransform fill;      // stretched inside the bar; its right anchor tracks health
    public GameObject barRoot;      // label + bar, toggled off while no boss exists
    public GameObject tutorialRoot; // tutorial text, shown until the boss replaces it

    private BossHealth boss;

    void Start()
    {
        if (barRoot != null) barRoot.SetActive(false);
        if (tutorialRoot != null) tutorialRoot.SetActive(true);
    }

    void Update()
    {
        if (boss == null)
        {
            boss = FindFirstObjectByType<BossHealth>();
        }

        bool bossPresent = boss != null;

        if (barRoot != null && barRoot.activeSelf != bossPresent)
        {
            barRoot.SetActive(bossPresent);
        }

        if (tutorialRoot != null && tutorialRoot.activeSelf == bossPresent)
        {
            tutorialRoot.SetActive(!bossPresent);
        }

        if (bossPresent && fill != null)
        {
            Vector2 anchorMax = fill.anchorMax;
            anchorMax.x = Mathf.Clamp01(boss.HealthPercent);
            fill.anchorMax = anchorMax;
        }
    }
}
