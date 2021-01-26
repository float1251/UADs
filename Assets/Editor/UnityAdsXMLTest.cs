using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnityAdsXMLTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void UnityAdsXMLTestSimplePasses() {
            #if UNITY_IOS
            var xml = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/UAds/Editor/skadnetworks.plist.xml").text;
            var res = UAds.Editor.UnityAdsSKAdNetworkXmlParser.Parse(xml);
            Assert.IsNotEmpty(res);
            Assert.IsNotEmpty(res[0].skAdNetworkIdentifier);
            #endif
        }

    }
}
