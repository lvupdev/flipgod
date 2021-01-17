using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownText : MonoBehaviour
{
	public Text countdown;
	Structure st;
	public GameObject parent;


	void Start()
	{
		st = FindObjectOfType<Structure>();
		Transform parentTransform = this.transform.parent;
		parentTransform = this.transform;
		Transform childTransform = transform.Find("Countdown");
		childTransform.position = new Vector3(0, 0, 0); 
	}

	void Update()
    {
        if (st.isFreezed)
        {	
			countdown.text = st.delta.ToString();
		}
        else
        {
			countdown.text = null;
        }
	}



}
