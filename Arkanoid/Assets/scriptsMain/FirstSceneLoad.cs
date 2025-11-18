using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FirstSceneLoad : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private int _firstSceneId = 1;
    [SerializeField] private int _targetFPS = 120;

    [Header("Loading Animation")]
    [SerializeField] private Transform _loadingIcon;
    [SerializeField] private float _rotationSpeed = 180f;

    void Start()
    {
        Application.targetFrameRate = _targetFPS;

        StartCoroutine(LoadFirstScene());
    }

    void Update()
    {
        if (_loadingIcon != null)
        {
            _loadingIcon.Rotate(0, 0, -_rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator LoadFirstScene()
    {
        yield return null;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_firstSceneId);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        asyncLoad.allowSceneActivation = true;
    }
}
