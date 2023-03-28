using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Mirror;

public class MenadzerIgre : MonoBehaviour
{
    [SerializeField] List<GameObject> karte;
    List<GameObject> sveKarte;
    Text natpis;
    static MenadzerIgre instanca;

    void Awake()
    {
        gameObject.SetActive(SceneManager.GetActiveScene().name == "Postolje");

        if (instanca == null)
        {
            instanca = this;
            this.sveKarte = this.karte;
            this.natpis = GameObject.Find("BrojKarata").GetComponent<Text>();

            PlayerPrefs.SetInt("karte", this.sveKarte.Count);

            PlayerPrefs.DeleteKey("igrac1");
            PlayerPrefs.DeleteKey("igrac2");
            PlayerPrefs.DeleteKey("pobednik");
        }
    }

    void Update()
    {
        if (sveKarte.Count != 0)
        {
            this.natpis.text = "Broj karata: " + PlayerPrefs.GetInt("karte");
            return;
        }

        int igraciBezKarata = 0;
        foreach (MenadzerIgraca igrac in MenadzerIgraca.igraci)
            igraciBezKarata += igrac.NemaKarata() ? 1 : 0;

        if (igraciBezKarata != 2)
            return;

        SceneManager.LoadScene("Odjava");
    }

    public static MenadzerIgre Instanca => instanca;

    public List<GameObject> SveKarte => this.sveKarte;
}