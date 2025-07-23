using UnityEngine;

public class GiocatoreAnimazione : MonoBehaviour
{

    private const string STA_CAMMINANDO = "StaCamminando"; // Così facendo siamo più sicuri quando scriviamo la componente all'ultima riga perché, se sbagliamo a scrivere, ce lo sottolinea di rosso

    [SerializeField] private Giocatore giocatore; // Abbiamo serializzato perché in questo modo nell'Editor abbiamo inserito lo script Giocatore.cs nella Visual insieme a questo script, in questo modo possiamo riferirci alla classe Giocatore attraverso la variabile giocatore

    private Animator animator; // Leggere la documentazione
    private void Awake() {
        animator = GetComponent<Animator>(); // Abbiamo un riferimento al nostro componente Animator
    }
    private void Update() {
        animator.SetBool(STA_CAMMINANDO, giocatore.StaCamminando());  // "StaCamminando" è vero se il metodo invocato è vero, altrimenti è falso e quindi non sta camminando
    }
}
