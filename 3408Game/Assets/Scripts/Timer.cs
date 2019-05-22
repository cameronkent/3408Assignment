using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text textTime;
    public float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        textTime.text = ((int)time).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        textTime.text = ((int)time).ToString();
    }
}
