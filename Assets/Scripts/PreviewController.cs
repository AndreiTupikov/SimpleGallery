using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PreviewController : MonoBehaviour
{
    public int ID;
    [SerializeField] private Sprite error;
    private bool initialized;

    private void Update()
    {
        if (!initialized && ID > 0)
        {
            initialized = true;
            StartCoroutine("DownloadImage");
        }
    }

    private IEnumerator DownloadImage()
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture($"http://data.ikppbb.com/test-task-unity-data/pics/{ID.ToString()}.jpg");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = error;
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find("GalleryManager").GetComponent<GalleryManager>().LoadingReport();
    }
}
