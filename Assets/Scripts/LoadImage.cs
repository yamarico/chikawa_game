using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadImage : MonoBehaviour
{
    // string path = "/Users/shuhei/Downloads/image/";
    // [SerializeField] Image image;
    // string fileName = "image1";
    
    // // Start is called before the first frame update
    // void Start()
    // {
    //     Sprite sprite = loadImage();
    //     image.sprite = sprite;
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // Sprite loadImage()
    // {
    //     byte[] bytes = File.ReadAllBytes(path);
    //     Texture2D texture = new Texture2D(2, 2);
    //     texture.LoadImage(bytes);
    //     Rect rect = new Rect(0f, 0f, texture.width, texture.height);
    //     Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
    //     byte[] pngbytes = texture.EncodeToPNG();
    //     string fullpath = Path.Combine(Application.dataPath, "Resources", fileName + ".png");
    //     File.WriteAllBytes(fullpath, pngbytes);
    //     Debug.Log("Texture saved to: " + fullpath);
    //     return sprite;
    // }
}
