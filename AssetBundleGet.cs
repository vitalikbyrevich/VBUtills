namespace VBUtills;

public static class AssetBundleGet
{
    private static Dictionary<string, AssetBundle> _assetBundles = new Dictionary<string, AssetBundle>();

        // Основной метод для получения ассет-бандла
        private static AssetBundle GetAssetBundle(string bundleName)
        {
            if (!_assetBundles.TryGetValue(bundleName, out AssetBundle assetBundle))
            {
                try
                {
                    assetBundle = AssetUtils.LoadAssetBundleFromResources(bundleName, Assembly.GetExecutingAssembly());
                    if (assetBundle)
                    {
                        _assetBundles.Add(bundleName, assetBundle);
                        Debug.Log($"Successfully loaded AssetBundle: {bundleName}");
                    }
                    else
                    {
                        Debug.LogError($"Failed to load AssetBundle '{bundleName}'");
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error loading AssetBundle '{bundleName}': {e.Message}");
                    return null;
                }
            }
            return assetBundle;
        }

        // Метод для предварительной регистрации бандлов (опционально)
     /*   public static void RegisterAssetBundle(string bundleName, AssetBundle assetBundle)
        {
            if (assetBundle && !_assetBundles.ContainsKey(bundleName))
            {
                _assetBundles.Add(bundleName, assetBundle);
                Debug.Log($"Pre-registered AssetBundle: {bundleName}");
            }
        }*/

        // Универсальный метод загрузки префаба из любого бандла
        public static T LoadPrefabFromBundles<T>(string bundleName, string prefabName) where T : UnityEngine.Object
        {
            var assetBundle = GetAssetBundle(bundleName);
            if (!assetBundle) return null;

            var prefab = assetBundle.LoadAsset<T>(prefabName);
            if (!prefab)
            {
                Debug.LogWarning($"Prefab '{prefabName}' not found in bundle '{bundleName}'");
                return null;
            }
            return prefab;
        }
}