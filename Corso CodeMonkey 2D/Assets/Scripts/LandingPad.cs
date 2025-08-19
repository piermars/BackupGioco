using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;

    public int ScoreMultiplier() { // poichè abbiamo reso il multiplier privato, creiamo una funzione pubblica che ritorna questo valore in modo tale da poterlo utilizzare in altre classi SOLO quando invochiamo la funzione di questa classe.
        return scoreMultiplier;
    }
}
