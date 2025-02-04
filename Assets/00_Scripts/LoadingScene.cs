using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene instance = null;
    private void Awake()
    {
        if(instance == null) instance = this;
    }
    private GameObject sliderParent;
    public Slider slider;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI percentageText;
    public GameObject TouchToStartOBJ;
    private AsyncOperation asyncOperation; // 비동기로 미리 씬 로드

    private void Start()
    {
        versionText.text = "App Version." + Application.version;
        sliderParent = slider.transform.parent.gameObject;
    }

    private void Update()
    {
        if(asyncOperation != null)
        if(asyncOperation.progress >= 0.9f && Input.GetMouseButtonDown(0))
        {
            asyncOperation.allowSceneActivation = true;
            Base_Manager.GetGameStart = true;
        }
    }

    public void LoadingMain()
    {
        StartCoroutine(LoadDataCoroutine());
    }
    
    IEnumerator LoadDataCoroutine()
    {
        asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false; // 보관한 씬이 true일때만 반환, 씬 자동 전환 방지


        while(asyncOperation.progress < 0.9f) 
        {
            LoadingUpdate(asyncOperation.progress);
            yield return null;
        }
        LoadingUpdate(1.0f);
        TouchToStartOBJ.SetActive(true);
    }

    private void LoadingUpdate(float progress)
    {
        slider.value = progress;
        percentageText.text = string.Format("데이터를 가져오고 있습니다... <color=#FFFF00>{0}%", progress * 100.0f);
    }
    
}
