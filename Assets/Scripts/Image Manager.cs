using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ImageManager : MonoBehaviour
{
    public RawImage[] image;
    

    public string[] webpages;
    private int webCount;

    public Dictionary<string, Texture2D> savedImages;
    int numb;
    public void Start()
    {
        savedImages = new Dictionary<string, Texture2D>();
        savedImages.Clear();
        for (int i = 0; i < image.Length; i++) 
        {
            webCount = Random.Range(0, 3);
            GetWebImage(AddImage);            
        }
    }
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webpages[webCount]);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
        savedImages.Add(webpages[webCount], DownloadHandlerTexture.GetContent(request));
    }
    
    public void GetWebImage(Action<Texture2D> callback)
    {
        if (savedImages.ContainsKey(webpages[webCount]))
        {
            callback(savedImages[webpages[webCount]]);           
        }
        else
        {
            StartCoroutine(DownloadImage(AddImage));
        }
    }

    public void AddImage(Texture2D texture)
    {
        numb++;
        image[numb - 1].texture = texture;
    }
}
