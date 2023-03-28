using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FitResolution : MonoBehaviour
{
    AspectRatioFitter f;

    void Start() => this.f = GetComponent<AspectRatioFitter>();

    void Update() => this.f.aspectRatio = (float)Screen.width / Screen.height;

    void PromeniScenu(string scena) => SceneManager.LoadSceneAsync(scena);

    void Onemoguci()
    {
        if (PlayerPrefs.GetInt("igraPokrenuta") == 1)
            Destroy(gameObject);
    }

    void OnemoguciAnimaciju()
    {
        PlayerPrefs.SetInt("igraPokrenuta", 1);
        Destroy(gameObject);
    }
}