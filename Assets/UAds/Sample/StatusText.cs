using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{

	[SerializeField]
	Text text;

	public void UpdateStatus(string text)
	{
		var msg = string.Format("Status: " + text);
		Debug.Log(msg);
		this.text.text = msg;
	}

}
