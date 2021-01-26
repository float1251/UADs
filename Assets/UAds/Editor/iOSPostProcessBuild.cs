using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#if UNITY_IOS
using System.Xml;
using UnityEditor.iOS.Xcode;
namespace UAds.Editor {

    public class IOSPostProcessBuild {

        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
            if (target != BuildTarget.iOS)
                return;

            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // SKAdNetworkIdentifierのListを取得
            var xml = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/UAds/Editor/skadnetworks.plist.xml").text;
            var unityAdsSkAdNetworkItems = UnityAdsSKAdNetworkXmlParser.Parse(xml);
            var adcolonySkAdNetworkItems = new HashSet<string>() {
                // adcolony
                // https://support.adcolony.com/helpdesk/network-ids-for-skadnetwork-ios-only/
                "4pfyvq9l8r.skadnetwork",
                "yclnxrl5pm.skadnetwork",
                "v72qych5uu.skadnetwork",
                "tl55sbb4fm.skadnetwork",
                "t38b2kh725.skadnetwork",
                "prcb7njmu6.skadnetwork",
                "ppxm28t8ap.skadnetwork",
                "mlmmfzh3r3.skadnetwork",
                "klf5c3l5u5.skadnetwork",
                "hs6bdukanm.skadnetwork",
                "c6k4g5qg8m.skadnetwork",
                "9t245vhmpl.skadnetwork",
                "9rd848q2bz.skadnetwork",
                "8s468mfl3y.skadnetwork",
                "7ug5zh24hu.skadnetwork",
                "4fzdc2evr5.skadnetwork",
                "4468km3ulz.skadnetwork",
                "3rd42ekr43.skadnetwork",
                "2u9pt9hc89.skadnetwork",
                "m8dbw4sv7c.skadnetwork",
                "7rz58n8ntl.skadnetwork",
                "ejvt5qm6ak.skadnetwork",
                "5lm9lj6jb7.skadnetwork",
                "44jx6755aq.skadnetwork",
                "mtkv5xtk9e.skadnetwork",
            };

            // UAdsの設定を取得する
            var settings = UAdsSettingHelper.LoadOrCreateUAdsSettings();

            // 有効になっているものだけ追加するようにする
            var res = new HashSet<string>();
            if(settings.enableUnityMonetization)
                res.UnionWith(unityAdsSkAdNetworkItems.Select(v => v.skAdNetworkIdentifier));
            if(settings.enableAdcolony)
                res.UnionWith(adcolonySkAdNetworkItems);

            // plistに追加
            var rootDict = plist.root;
            var array = rootDict.CreateArray("SKAdNetworkItems");
            foreach (var item in res) {
                var dic = array.AddDict();
                dic.SetString("SKAdNetworkIdentifier", item);
            }
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }

    public static class UnityAdsSKAdNetworkXmlParser {

        public class SKAdNetworkItem {
            public string skAdNetworkIdentifier { get; private set; }

            public SKAdNetworkItem(string skAdNetworkIdentifier) {
                this.skAdNetworkIdentifier = skAdNetworkIdentifier;
            }

        }
        
        public static List<SKAdNetworkItem> Parse(string xmlText) {
            /// var xml = XDocument.Load("https://skan.mz.unity3d.com/v2/partner/skadnetworks.plist.xml");
            var xml = new XmlDocument();
            xml.LoadXml(xmlText);
            var elements= xml.SelectNodes("plist/array/dict");
            var res = new List<SKAdNetworkItem>();
            foreach(XmlNode el in elements) {
                var val = el.SelectSingleNode("string").InnerText;
                res.Add(new SKAdNetworkItem(val));
                Debug.Log(val);
            }

            return res;
        }
    }
}
#endif
