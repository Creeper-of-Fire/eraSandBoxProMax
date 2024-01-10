using System.Collections.Generic;

namespace eraSandBox.Coitus
{
    public static class DataUtility
    {
        /// <summary>
        /// 如果Key重复就覆盖
        /// </summary>
        public static void AddAndCover<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }
            dictionary.Add(key, value);
        }
        /// <summary>
        /// 如果Key重复就跳过
        /// </summary>
        public static void AddAndSkip<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                return;
            }
            dictionary.Add(key, value);
        }
    }
}