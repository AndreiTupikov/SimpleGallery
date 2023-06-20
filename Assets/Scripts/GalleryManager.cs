using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadedPersentImage;
    [SerializeField] private Transform cameraView;
    [SerializeField] private GameObject previewPrefab;
    [SerializeField] private float firstLineHeight;
    [SerializeField] private float previewDistance;
    [SerializeField] private int imageLimit;
    private Text loadedPersentText;
    private bool initialLoadingEnded = false;
    private int loadedPreviews = 0;
    private float cameraHighestPosition;
    private float cameraLowestPosition = 1;
    private int nextPreviewNumber = 1;
    private float lastPreviewsLineHeight;
    private bool rightSide = false;
    private float touchStartingTime = -1;
    private Vector2 touchStarting;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        loadedPersentText = loadingPanel.GetComponentInChildren<Text>();
        cameraHighestPosition = cameraView.position.y;
        lastPreviewsLineHeight = cameraHighestPosition + firstLineHeight + previewDistance;
    }

    private void Update()
    {
        if (initialLoadingEnded)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    touchStartingTime = Time.time;
                    touchStarting = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                if (touchStartingTime > 0)
                {
                    float scroll = touchStarting.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                    if (cameraView.position.y > cameraHighestPosition && scroll > 0) scroll = 0;
                    else if (cameraLowestPosition < 0 && cameraView.position.y < cameraLowestPosition && scroll < 0) scroll = 0;
                    cameraView.position = new Vector3(cameraView.position.x, cameraView.position.y + scroll, cameraView.position.z);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time < touchStartingTime + 0.15f)
                {
                    Vector2 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(touch, Vector2.zero);
                    if (hit.collider != null && Vector2.Distance(touch, touchStarting) < 0.1f)
                    {
                        DataHolder.loadingImage = hit.transform.GetComponent<SpriteRenderer>().sprite;
                        SceneManager.LoadScene("ImageViewScene");
                    }
                }
                touchStartingTime = -1;
            }
        }
    }

    private void FixedUpdate()
    {
        if (nextPreviewNumber <= imageLimit && (rightSide || cameraView.position.y - lastPreviewsLineHeight < firstLineHeight))
        {
            AddPreviewsLine(nextPreviewNumber++);
            if (nextPreviewNumber > imageLimit) cameraLowestPosition = lastPreviewsLineHeight + firstLineHeight;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene("MenuScene");
        }
    }

    public void LoadingReport()
    {
        if (!initialLoadingEnded)
        {
            loadedPreviews++;
            float loadedPart = loadedPreviews / ((float)nextPreviewNumber - 1);
            loadedPersentImage.fillAmount = loadedPart;
            loadedPersentText.text = $"{((int)(loadedPart * 100)).ToString()}%";
            if (nextPreviewNumber == loadedPreviews + 1) StartCoroutine(ShowGallery());
        }
    }

    private void AddPreviewsLine(int id)
    {
        float xPosition = previewDistance / 2;
        if (!rightSide)
        {
            lastPreviewsLineHeight -= previewDistance;
            xPosition *= -1;
        }
        rightSide = !rightSide;
        GameObject preview = Instantiate(previewPrefab, new Vector2(xPosition, lastPreviewsLineHeight), Quaternion.identity);
        preview.GetComponent<PreviewController>().ID = id;
    }

    private IEnumerator ShowGallery()
    {
        yield return new WaitForSeconds(0.3f);
        initialLoadingEnded = true;
        loadingPanel.SetActive(false);
    }
}