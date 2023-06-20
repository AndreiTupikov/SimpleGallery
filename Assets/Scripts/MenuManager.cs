using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        DataHolder.screenWidth = Screen.width;
        DataHolder.screenHeight = Screen.height;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void LoadGallery()
    {
        SceneManager.LoadScene("GalleryScene");
    }
}
