using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetBundleManager : MonoBehaviour
{
    AssetBundle assetBg;

    #region SINGLETON PATTERN
    public static AssetBundleManager _instance;
    public static AssetBundleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<AssetBundleManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("AssetBundleManager");
                    _instance = container.AddComponent<AssetBundleManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    private string GetBundlePathForLoadFromFile(string relativePath)
    {
        var streamingAssetsPath = Application.streamingAssetsPath;
        return Path.Combine(streamingAssetsPath, relativePath);
    }

    private AssetBundle LoadBundleFromStreamingAssets(string relativePath)
    {
        return AssetBundle.LoadFromFile(GetBundlePathForLoadFromFile(relativePath));
    }

    public Sprite GetSprite(string relativePath, string name)
    {
        Sprite image = null;
        if(assetBg == null)
            assetBg = LoadBundleFromStreamingAssets(relativePath);

        if(assetBg)
        {
            image = assetBg.LoadAsset<Sprite>(name);
        }
        return image;
    }

}
