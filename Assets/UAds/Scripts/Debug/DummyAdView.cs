using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UAds.Editor
{
	public class DummyAdView : MonoBehaviour
	{
		public OnFinishRewardVideo onFinish;
		public void OnClickClose()
		{
			onFinish.Invoke(VideoAdStatus.Cancel);
			Destroy(gameObject);
		}

		public void OnClickFinish()
		{
			onFinish.Invoke(VideoAdStatus.Success);
			Destroy(gameObject);
		}
	}

}