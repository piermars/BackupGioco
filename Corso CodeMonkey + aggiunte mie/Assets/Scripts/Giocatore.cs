using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class Giocatore : MonoBehaviour {
    // Rendere moveSpeed pubblico pu� causare malfunzionamento con altre classi che utilizzano la stessa variabile, motivo per la quale � pi� sicuro e comodo renderla private e inserirci SerializeField
    [SerializeField] private float moveSpeed = 6.5f; // Pu� essere comodo renderla pubblica all'inizio giusto per far comparire il valore nell'editor e trovare pi� comodamente la velocit� adatta
    [SerializeField] private InputGioco inputGioco; // Qui inseriremo lo script di InputGioco, altrimenti di default la variabile sar� nulla
    [SerializeField] private LayerMask countersLayerMask; // Serve per non interagire con qualcosa da dietro un muro

    private bool staCamminando; // Variabile che servir� sotto per l'animazione di camminata
    private Vector3 ultimaDirInterazione; // quando camminiamo verso un oggetto riusciamo a interagirci, quando lo guardiamo e ci fermiamo no. Questa var serve per permettere ci�, permettendo di tenere traccia dell'ultima direzione di movimento eseguita.

    private void Start() {
        inputGioco.SuInterazioneAzione += InputGioco_SuInterazioneAzione;
    }

    private void InputGioco_SuInterazioneAzione(object sender, EventArgs e) { // ovviamente clonare il codice in pi� parti non va bene, ma per ora lasciamolo cos�

        Vector2 inputVettore = inputGioco.OttieniVettoreMovimentoNormalizzato();

        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Non la rendiamo pubblica perch� poi si creerebbero casini

        if (dirGiocatore != Vector3.zero) {
            ultimaDirInterazione = dirGiocatore; // ultimaDirInterazione assume come valore l'ultimo movimento eseguito dal giocatore, � utile per migliorare il RayCast
        }

        float distanzaInterazione = 2f;
        if (Physics.Raycast(transform.position, ultimaDirInterazione, out RaycastHit raycastHit, distanzaInterazione, countersLayerMask)) { // out sta per output
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                // Ha un ClearCounter
                clearCounter.Interazione();
            }
        }
    }

    // Movimento del personaggio
    private void Update() { // Update is called once per frame
        GestioneMovimento();
        GestioneInterazione();
    }
    public bool StaCamminando() {
        return staCamminando;
    }
    private void GestioneInterazione() {
        Vector2 inputVettore = inputGioco.OttieniVettoreMovimentoNormalizzato();
        
        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Non la rendiamo pubblica perch� poi si creerebbero casini

        if (dirGiocatore != Vector3.zero) {
            ultimaDirInterazione = dirGiocatore; // ultimaDirInterazione assume come valore l'ultimo movimento eseguito dal giocatore, � utile per migliorare il RayCast
        }

        float distanzaInterazione = 2f;
        if (Physics.Raycast(transform.position, ultimaDirInterazione, out RaycastHit raycastHit, distanzaInterazione, countersLayerMask)) { // out sta per output
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                // Ha un ClearCounter
                
            }
        } 
    }
    private void GestioneMovimento() {
        Vector2 inputVettore = inputGioco.OttieniVettoreMovimentoNormalizzato(); // Invochiamo la funzione dalla classe InputGioco

        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Creiamo questo vettore poich� la posizione del giocatore va tra x,y,z e non solo x,y

        // Collisioni giocatore + evitare di muoversi mentre si bacia il muro
        float distanzaMovimento = moveSpeed * Time.deltaTime; // Con l'ultimo costrutto, evitiamo che la velocit� di un giocatore vada in base ai suoi fps
        float altezzaGiocatore = 2f;
        float raggioGiocatore = .7f;
        bool pu�Muoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatore, distanzaMovimento);

        if (!pu�Muoversi) { // non pu� muoversi verso dirGiocatore, proviamo SOLO verso x
            Vector3 dirGiocatoreX = new Vector3(dirGiocatore.x, 0, 0).normalized; // per evitare che si muova su un muro quando si schiaccia WD o WA insieme
            pu�Muoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatoreX, distanzaMovimento);

            if (pu�Muoversi) {
                dirGiocatore = dirGiocatoreX;
            } else {
                // non pu� muoversi solo sull'asse X, proviamo su z
                Vector3 dirGiocatoreZ = new Vector3(0, 0, dirGiocatore.z).normalized;
                pu�Muoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatoreZ, distanzaMovimento);

                if (pu�Muoversi) {
                    dirGiocatore = dirGiocatoreZ;
                } else {
                    // Non pu� muoversi
                }
            }
        }

        if (pu�Muoversi) {
            transform.position += dirGiocatore * distanzaMovimento;
        }

        staCamminando = dirGiocatore != Vector3.zero; // il giocatore si muove se la direzione del giocatore � diversa dal vettore zero

        // Rotazione giocatore
        float rotateSpeed = 10f; // Velocit� di rotazione
        transform.forward = Vector3.Slerp(transform.forward, dirGiocatore, Time.deltaTime * rotateSpeed);
    }

}
