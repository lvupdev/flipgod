using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    // Variable about scroll bar
    public GameObject scrollbarObj;
    private Scrollbar scrollbar;

    // Current position of scroll bar
    private float scrollPos;

    // Positions of every button
    private float[] buttonPos;

    private void Start()
    {
        // Init
        scrollbar = scrollbarObj.GetComponent<Scrollbar>();
    }

    private void Update()
    {
        buttonPos = new float[transform.childCount];
        float distance = 1f / (buttonPos.Length - 1f);
        for (int i = 0; i < buttonPos.Length; i++)
        {
            buttonPos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollbar.value;
        }
        else
        {
            for (int i = 0; i < buttonPos.Length; i++)
            {
                if (scrollPos < buttonPos[i] + (distance / 2) && scrollPos > buttonPos[i] - (distance / 2))
                {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, buttonPos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < buttonPos.Length; i++)
        {
            if (scrollPos < buttonPos[i] + (distance / 2) && scrollPos > buttonPos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);

                for (int j = 0; j < buttonPos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}
