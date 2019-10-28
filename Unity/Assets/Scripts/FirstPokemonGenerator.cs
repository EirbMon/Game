using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPokemonGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Pikachu;
    public GameObject Carapuce;
    public GameObject Salameche;

    void Start()
    {
        GameObject PikachuClone = Instantiate(Pikachu, new Vector2(-56.5f, 4.1f), Quaternion.identity);
        GameObject CarapuceClone = Instantiate(Carapuce, new Vector2(-57.44f, 4.2f), Quaternion.identity);
        GameObject SalamecheClone = Instantiate(Salameche, new Vector2(-55.5f, 4.1f), Quaternion.identity);

        PikachuClone.GetComponent<PokemonObject>().name = "PikaPika";
        CarapuceClone.GetComponent<PokemonObject>().name = "CaraCara";
        SalamecheClone.GetComponent<PokemonObject>().name = "SalaSala";


    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
