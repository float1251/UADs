using UnityEngine;
using System.Collections;
#if ENABLE_ADCOLONY
using AdColony;
#endif

namespace UAds.Sample
{

	public class SampleController : MonoBehaviour
	{

		UAds.IVideoAdvertisement ads;

		public void OnClickUnityAds()
		{

		}

		public void OnClickAdcolony()
		{
#if ENABLE_ADCOLONY
			this.ads = new UAds.UADAdColony("", "", true);
#endif
		}

		public void OnClickInitialize()
		{
			this.ads.Initialize();
		}

	}
}
