using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class Giocatore : MonoBehaviour {
    // Rendere moveSpeed pubblico può causare malfunzionamento con altre classi che utilizzano la stessa variabile, motivo per la quale è più sicuro e comodo renderla private e inserirci SerializeField
    [SerializeField] private float moveSpeed = 6.5f; // Può essere comodo renderla pubblica all'inizio giusto per far comparire il valore nell'editor e trovare più comodamente la velocità adatta
    [SerializeField] private InputGioco inputGioco; // Qui inseriremo lo script di InputGioco, altrimenti di default la variabile sarà nulla
    [SerializeField] private LayerMask countersLayerMask; // Serve per non interagire con qualcosa da dietro un muro

    private bool staCamminando; // Variabile che servirà sotto per l'animazione di camminata
    private Vector3 ultimaDirInterazione; // quando camminiamo verso un oggetto riusciamo a interagirci, quando lo guardiamo e ci fermiamo no. Questa var serve per permettere ciò, permettendo di tenere traccia dell'ultima direzione di movimento eseguita.

    private void Start() {
        inputGioco.SuInterazioneAzione += InputGioco_SuInterazioneAzione;
    }

    private void InputGioco_SuInterazioneAzione(object sender, EventArgs e) { // ovviamente clonare il codice in più parti non va bene, ma per ora lasciamolo così

        Vector2 inputVettore = inputGioco.OttieniVettoreMovimentoNormalizzato();

        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Non la rendiamo pubblica perchè poi si creerebbero casini

        if (dirGiocatore != Vector3.zero) {
            ultimaDirInterazione = dirGiocatore; // ultimaDirInterazione assume come valore l'ultimo movimento eseguito dal giocatore, è utile per migliorare il RayCast
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
        
        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Non la rendiamo pubblica perchè poi si creerebbero casini

        if (dirGiocatore != Vector3.zero) {
            ultimaDirInterazione = dirGiocatore; // ultimaDirInterazione assume come valore l'ultimo movimento eseguito dal giocatore, è utile per migliorare il RayCast
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

        Vector3 dirGiocatore = new Vector3(inputVettore.x, 0f, inputVettore.y); // Creiamo questo vettore poiché la posizione del giocatore va tra x,y,z e non solo x,y

        // Collisioni giocatore + evitare di muoversi mentre si bacia il muro
        float distanzaMovimento = moveSpeed * Time.deltaTime; // Con l'ultimo costrutto, evitiamo che la velocità di un giocatore vada in base ai suoi fps
        float altezzaGiocatore = 2f;
        float raggioGiocatore = .7f;
        bool puòMuoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatore, distanzaMovimento);

        if (!puòMuoversi) { // non può muoversi verso dirGiocatore, proviamo SOLO verso x
            Vector3 dirGiocatoreX = new Vector3(dirGiocatore.x, 0, 0).normalized; // per evitare che si muova su un muro quando si schiaccia WD o WA insieme
            puòMuoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatoreX, distanzaMovimento);

            if (puòMuoversi) {
                dirGiocatore = dirGiocatoreX;
            } else {
                // non può muoversi solo sull'asse X, proviamo su z
                Vector3 dirGiocatoreZ = new Vector3(0, 0, dirGiocatore.z).normalized;
                puòMuoversi = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * altezzaGiocatore, raggioGiocatore, dirGiocatoreZ, distanzaMovimento);

                if (puòMuoversi) {
                    dirGiocatore = dirGiocatoreZ;
                } else {
                    // Non può muoversi
                }
            }
        }

        if (puòMuoversi) {
            transform.position += dirGiocatore * distanzaMovimento;
        }

        staCamminando = dirGiocatore != Vector3.zero; // il giocatore si muove se la direzione del giocatore è diversa dal vettore zero

        // Rotazione giocatore
        float rotateSpeed = 10f; // Velocità di rotazione
        transform.forward = Vector3.Slerp(transform.forward, dirGiocatore, Time.deltaTime * rotateSpeed);
    }

}
