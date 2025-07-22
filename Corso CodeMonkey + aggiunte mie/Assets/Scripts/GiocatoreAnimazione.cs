using UnityEngine;

public class GiocatoreAnimazione : MonoBehaviour
{

    private const string STA_CAMMINANDO = "StaCamminando"; // Cos� facendo siamo pi� sicuri quando scriviamo la componente all'ultima riga perch�, se sbagliamo a scrivere, ce lo sottolinea di rosso


    private Animator animator; // Leggere la documentazione
    private void Awake() {
        animator = GetComponent<Animator>(); // Abbiamo un riferimento al nostro componente Animator (parametro?)
        animator.SetBool(STA_CAMMINANDO, // continuare da 1:38:11 circa
    }
}
