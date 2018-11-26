using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UAds.Utils
{
    public static class ListExtention
    {
        /// <summary>
        /// ランダムに並び替え
        /// Fisher-Yatesアルゴリズム
        /// </summary>
        public static List<T> Shuffle<T>(this List<T> list)
        {

            for (int i = 0, count = list.Count; i < count; i++)
            {
                T temp = list[i];
                int randomIndex = UnityEngine.Random.Range(0, count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }
    }
}
