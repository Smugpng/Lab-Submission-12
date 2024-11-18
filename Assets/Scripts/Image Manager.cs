using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ImageManager : MonoBehaviour
{
    public RawImage image;
    private const string webImage = "https://upload.wikimedia.org/wikipedia/commons/6/6a/The_birthday_cake_LCCN2013648266.jpg";
    private const string webImage1 = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e8/A_cat_cleaning_its_claws_LCCN2009631922.tif/lossy-page1-800px-A_cat_cleaning_its_claws_LCCN2009631922.tif.jpg?20180404072544";
    private const string webImage2 = "https://commons.wikimedia.org/wiki/File:Southern_Hairy-nosed_Wombat.jpg#/media/File:Southern_Hairy-nosed_Wombat.jpg";
    public void Start()
    {
        int rnd = Random.Range(1, 3);
        switch (rnd)
        {
            case 1:
                DownloadImage(AddImage,webImage);
                break;
            case 2:
                DownloadImage(AddImage,webImage1);
                break;
            case 3:
                DownloadImage(AddImage, webImage2);
                break;
        }
    }
    public IEnumerator DownloadImage(Action<Texture2D> callback, string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));

    }
    public void AddImage(Texture2D texture)
    {
        Debug.Log("what");
        image.texture = texture;
    }
}
