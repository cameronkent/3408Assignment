using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Image keyImage;
    public bool hasKey;

    void Start()
    { 

    }

    void FixedUpdate()
    {
        float alphaValue;
        if (hasKey)
        {
            alphaValue = 1f;
        }
        else
        {
            alphaValue = 0f;
        }
        keyImage.color = new Color(keyImage.color.r, keyImage.color.g, keyImage.color.b, alphaValue);
    }
}