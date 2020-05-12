using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField]
    private float delay;
    [SerializeField]
    private int maxBgCount;

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<Sprite> sprites;

    private void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        if (sprites == null)
            sprites = new List<Sprite>();

        Initialize();
    }

    private void Initialize()
    {
        UpdateBackGround();
        StartCoroutine(ChangeBGInDelay(delay));
    }

    private void UpdateBackGround()
    {
        for (int i = 1; i <= maxBgCount; i++) {
            sprites.Add(AssetBundleManager.Instance.GetSprite("sprite/bg","BG"+i));
        }
    }

    IEnumerator ChangeBGInDelay(float delay)
    {
        int i = 0;
        Sprite bgImage;
        while (spriteRenderer)
        {
            yield return new WaitForSeconds(delay);

            i = Random.RandomRange(0, maxBgCount - 1);
            if (sprites.Count >= 0)
            {
                bgImage = sprites[i];
                if (bgImage)
                    spriteRenderer.sprite = bgImage;
            }

        }
    }
}
