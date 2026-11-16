using UnityEngine;



public class VictoryChecker : MonoBehaviour
{
    public GameObject Voittoteksti;
    public bool Voitettu = false;   
    public int VoittojenMaara = 0;
     void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("VictoryCollider"))
        {
            Voitettu = true;
            VoittojenMaara ++;
            other.gameObject.SetActive(true);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Voittoteksti.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Voitettu){
            Voittoteksti.SetActive(true);
        }
    }
}
