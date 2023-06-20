using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageViewManager : MonoBehaviour
{
    [SerializeField] RectTransform image;
    private DeviceOrientation orientation;
    private bool initialized;
    private Vector2 portraitSize;
    private Vector2 landscapeSize;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        orientation = DeviceOrientation.Unknown;
        SizesInitialize(DataHolder.loadingImage.texture.height, DataHolder.loadingImage.texture.width);
        image.GetComponent<Image>().sprite = DataHolder.loadingImage;
    }

    private void FixedUpdate()
    {
        if (initialized && orientation != Input.deviceOrientation)
        {
            ChangeImageSize(Input.deviceOrientation);
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape)) BackToGallery();
        }
    }

    public void BackToGallery()
    {
        SceneManager.LoadScene("GalleryScene");
    }

    private void ChangeImageSize(DeviceOrientation ori)
    {
        switch (ori)
        {
            case DeviceOrientation.Portrait:
            case DeviceOrientation.PortraitUpsideDown:
                image.sizeDelta = portraitSize;
                break;
            case DeviceOrientation.LandscapeLeft:
            case DeviceOrientation.LandscapeRight:
                image.sizeDelta = landscapeSize;
                break;
            default:
                break;
        }
        orientation = ori;
    }

    private void SizesInitialize(float imageHeight, float imageWidth)
    {
        float imageRatio = imageHeight / imageWidth;
        portraitSize = new Vector2 (DataHolder.screenWidth, DataHolder.screenWidth * imageRatio);
        if (portraitSize.y > DataHolder.screenHeight) portraitSize = new Vector2(DataHolder.screenHeight / imageRatio, DataHolder.screenHeight);
        landscapeSize = new Vector2(DataHolder.screenHeight, DataHolder.screenHeight * imageRatio);
        if (landscapeSize.y > DataHolder.screenWidth) landscapeSize = new Vector2(DataHolder.screenWidth / imageRatio, DataHolder.screenWidth);
        if (Input.deviceOrientation == DeviceOrientation.FaceDown || Input.deviceOrientation == DeviceOrientation.FaceUp) ChangeImageSize(DeviceOrientation.Portrait);
        initialized = true;
    }
}