using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] List<string> names;

    [SerializeField] List<Material> materials;

    [SerializeField] Image loadingBar;
    [SerializeField] MeshRenderer groundMeshRenderrer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDownloadingByNames(names));
    }

    public IEnumerator StartDownloadingByNames(List<string> names)
    {
        // check whether have to download or not
        long totalDownloadSize = 0;

        // calc total download size
        foreach (string name in names)
        {
            AsyncOperationHandle<long> sizeHandle = Addressables.GetDownloadSizeAsync(name);
            yield return sizeHandle;

            if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
            {
                yield break;
            }
            totalDownloadSize += sizeHandle.Result;
        }
        Debug.Log("totalDownloadSize" + totalDownloadSize);
        if (totalDownloadSize > 0)
        {
            foreach (string name in names)
                yield return StartDownloadingName(name);
        }
        else
        {
            Debug.Log("using cache.");
            LoadAddressablesByNames(names);   // download finish, now load them

            StartCoroutine(LoadingFill());
        }
    }

    public IEnumerator StartDownloadingName(string name)
    {
        AsyncOperationHandle downloadHandle = Addressables.DownloadDependenciesAsync(name);
        yield return StartCoroutine(LoadingFill(downloadHandle));
        yield return downloadHandle;

        if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Error...");
        }
        else
        {
            Debug.Log("Success, " + downloadHandle.Result);
            LoadAddressablesByNames(names);   // download finish, now load them
        }

    }
    public void LoadAddressablesByNames(List<string> names)
    {
        Debug.Log("Loading... called");
        StartCoroutine(LoadAddressablesCoroutine(names));
    }
    public IEnumerator LoadAddressablesCoroutine(List<string> names)
    {
        foreach (string name in names)
        {
            yield return LoadAddressablesName(name);
        }
    }

    private IEnumerator LoadAddressablesName(string name)
    {
        // if (name == "mat")
        // {
        AsyncOperationHandle<IList<Material>> handle = Addressables.LoadAssetsAsync<Material>(name, null);
        yield return handle;

        foreach (Material mat in handle.Result)
        {
            // Debug.Log("name === " + mat.name);
            materials.Add(mat);
        }
        groundMeshRenderrer.material = materials.FirstOrDefault(mat => mat.name == "Yellow");
        // }
    }

    public IEnumerator LoadingFill(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            float progress = handle.PercentComplete;
            loadingBar.fillAmount = progress;
            yield return null;
        }

        // StartCoroutine(LoadingFill());
    }
    public IEnumerator LoadingFill()
    {
        loadingBar.fillAmount = 1;
        yield return null;
    }
}

