using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SwitchImage : MonoBehaviour
{
    public Sprite laugh;
    public Sprite normal;
    public Sprite cry;

    private SpriteRenderer spriteRenderer;
    private PaientManager pm;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        pm = GetComponent<PaientManager>();
    }

    void Update()
    {
        // 根据属性值切换图片
        switch (pm.patientAttributes.mood)
        {
            case >70:
                spriteRenderer.sprite = laugh;
                break;
            case >40:
                spriteRenderer.sprite = normal;
                break;
            default:
                spriteRenderer.sprite = cry;
                break;
            // 你可以根据需要添加更多的情况
        }
    }
}
