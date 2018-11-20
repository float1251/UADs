using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{

	[SerializeField]
	Text text;

	public void UpdateStatus(string text)
	{
		this.text.text = string.Format("Status: " + text);
	}

}
