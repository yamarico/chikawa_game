using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class CountDown : MonoBehaviour
{
    public Text countdownText; // UIのテキストコンポーネント
    private int countdownTime = 3;

    void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1);
            countdownTime--;
        }

        countdownText.text = "撮影！";
        SendRequestToPython();
    }

    void SendRequestToPython()
    {
        try
        {
            // Pythonにリクエストを送信
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();

            // リクエストメッセージを送信
            byte[] requestMessage = Encoding.UTF8.GetBytes("capture");
            stream.Write(requestMessage, 0, requestMessage.Length);

            // 画像データを受信
            byte[] dataSizeBytes = new byte[4];
            stream.Read(dataSizeBytes, 0, 4);
            int dataSize = System.BitConverter.ToInt32(dataSizeBytes, 0);

            byte[] imageData = new byte[dataSize];
            int totalBytesReceived = 0;
            while (totalBytesReceived < dataSize)
            {
                int bytesReceived = stream.Read(imageData, totalBytesReceived, dataSize - totalBytesReceived);
                totalBytesReceived += bytesReceived;
            }

            // 画像をTexture2Dに変換
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            // 画像を表示（適切なUI要素に適用）
            // 例: RawImageコンポーネントに適用
            // rawImageComponent.texture = texture;

            stream.Close();
            client.Close();
        }
        catch (SocketException e)
        {
            Debug.Log("ソケットエラー: " + e.Message);
        }
    }
}
