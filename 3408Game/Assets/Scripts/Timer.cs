using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text textTime;
    public float timeLeft = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        textTime.text = ((int)timeLeft).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        textTime.text = ((int)timeLeft).ToString();
        if (timeLeft < 0)
        {

        }
    }
}
