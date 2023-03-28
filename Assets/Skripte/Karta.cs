using UnityEngine;

[CreateAssetMenu(fileName = "Karta", menuName = "Kreiraj Kartu")]
public class Karta : ScriptableObject
{
    public enum Znak { PIK, HERC, TREF, KARO }

    [SerializeField] Znak znak;
    [SerializeField] int broj;
    [SerializeField] Sprite sprajtKarte;

    public int Broj => this.broj;

    public Znak Simbol => this.znak;

    public Sprite Sprajt => this.sprajtKarte;
}