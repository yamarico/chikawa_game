using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine.SceneManagement;

// Pythonで画像を読み込んだ瞬間にUnityにテクスチャとして読み込む（LoadImage）
// テクスチャをSpriteに変換（Sprite Create）
// このSpriteを元に Create(Sprite img)関数を実行する→オブジェクトが生成される
public class CreateManager : MonoBehaviour
{
    private GameObject obj;
    public List<GameObject> people;//どうぶつ取得配列
    public bool isFall;
    int file_length;
    public float pivotHeight = 3;//生成位置の基準
    public Camera mainCamera;//カメラ取得用変数
    public GameObject cameracontroller;
    // Start is called before the first frame update
    void Init()
    {
        Animal.isMoves.Clear();//移動してる動物のリストを初期化
        string[] files = Directory.GetFiles(
            @"Assets/Resources", "*.png", SearchOption.AllDirectories
            ).ToArray();
        file_length = files.Length;
 //       obj = null;
    }

    void Start()
    {
        
        string[] files = Directory.GetFiles(
              @"Assets/Resources", "*.png", SearchOption.AllDirectories
              );
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
        Init();
    }

    // Update is called once per frame
    void Update()
    {
//        if (obj && obj.transform.position.y < -5)
//        {
//        }
        if (CheckGameOver(people)){
          SceneManager.LoadScene ("GameOver");
        }
        if (CheckMove(Animal.isMoves))
        {
            return;//移動中なら処理はここまで
        }
        // test
        if(Input.GetKeyDown(KeyCode.A)){
            CreateFromTestPicture();
        }


        //　読み込んだ画像が落ちてくるコードに変更する
        //　画像を読み込む関数＋Create関数
        string[] files = Directory.GetFiles(
            @"Assets/Resources", "*.png", SearchOption.AllDirectories
            ).OrderBy(f => File.GetLastWriteTime(f).Date
            ).ToArray();
        if (files.Length == 0){
            return;
        }
        if (files.Length > file_length)
        {
            string tar = files[files.Length - 1].Remove(0, 17);
            tar = tar.Replace(".png", "");
            //　ここで落とす画像を設定している
            Sprite img = Resources.Load(tar, typeof(Sprite)) as Sprite;
//            Debug.Log(img);
            if (img == null){
                return;
            }
            Create(img);
            file_length += 1;
        }
        /*Vector2 v = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, pivotHeight);

        if (Input.GetMouseButtonUp(0))//もし（マウス左クリックが離されたら）
        {
            if (!RotateButton.onButtonDown)//ボタンをクリックしていたら反応させない
            {
                obj.transform.position = v;
                obj.GetComponent<Rigidbody2D>().isKinematic = false;//――――物理挙動・オン
                isFall = true;//落ちて、どうぞ
            }
            RotateButton.onButtonDown = false;//マウスが上がったらボタンも離れたと思う
        }
        else if (Input.GetMouseButton(0))//ボタンが押されている間
        {
            obj.transform.position = v;
        }*/
    }

    void CreateFromTestPicture()
    {
        // 指定ディレクトリ内のすべての .png ファイルを取得し、最終更新日時でソートして最後のファイルを選択
        string directoryPath = "/Users/shuhei/Downloads/Image";
        string[] files = Directory.GetFiles(directoryPath, "*.png");

        // 最終更新日時でファイルをソートして最後のファイルを取得
        string latestFile = files.OrderBy(f => File.GetLastWriteTime(f)).Last();
        Debug.Log("最新の画像ファイル: " + latestFile);

        // ファイルを読み込み、Texture2D としてロード
        byte[] bytes = File.ReadAllBytes(latestFile);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        // Texture2D を Sprite に変換
        Rect rect = new Rect(0f, 0f, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
        Create(sprite);
    }

    void Create(Sprite img)
    {
        /*while (CameraController.isCollision)
        {
            Debug.Log("collision_start");
            cameracontroller.transform.Translate(0, 2.0f, 0);
            mainCamera.transform.Translate(0, 2.0f, 0);//カメラを少し上に移動
            pivotHeight += 2.0f;//生成位置も少し上に移動
        Debug.Log("collision_fin");
        }*/
        if (CameraController.isCollision)
        {
            Debug.Log("collision_start");
            cameracontroller.transform.Translate(0, 3.0f, 0);
            mainCamera.transform.Translate(0, 3.0f, 0);
            pivotHeight += 3.0f;
            Debug.Log("collision_fin");
        }
        isFall = false;
        obj = new GameObject();
        obj.AddComponent<SpriteRenderer>();
        obj.GetComponent<SpriteRenderer>().sprite = img;
        obj.AddComponent<PolygonCollider2D>();
        obj.AddComponent<Rigidbody2D>();
        //obj.GetComponent<Rigidbody2D>().isKinematic = true;
        obj.AddComponent<Animal>();
        obj.transform.position = new Vector3(0.0f, pivotHeight, 0.0f);
        people.Add(obj);
    }

    /// <summary>
    /// 移動中かチェック
    /// </summary>
    /// <param name="isMoves"></param>
    /// <returns></returns>
    bool CheckMove(List<Moving> isMoves)
    {
        if (isMoves == null)
        {
            return false;
        }
        foreach (Moving b in isMoves)
        {
            if (b.isMove)
            {
                //Debug.Log("移動中(*'ω'*)");
                return true;
            }
        }
        return false;
    }

    bool CheckGameOver(List<GameObject> people)
    {
        foreach (GameObject b in people)
        {
            if (b.transform.position.y < -5)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// どうぶつの回転
    /// ボタンにつけて使います
    /// </summary>
    public void RotateAnimal()
    {
        if (!isFall)
        {
            obj.transform.Rotate(0, 0, -30);//30度ずつ回転
        }
    }
}
