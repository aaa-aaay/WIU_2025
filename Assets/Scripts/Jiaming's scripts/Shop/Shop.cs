
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField] private GameObject shopCanvas;

    private void Start()
    {
        shopCanvas.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("hitting smth");
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if(player != null)
        {
            Debug.Log("found a player");
            if (Input.GetKey(KeyCode.E)) {

                //openShop
                Debug.Log("pressed and enabled");
                shopCanvas.SetActive(true);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInven player = other.gameObject.GetComponentInChildren<PlayerInven>();
        if(player != null) {
            shopCanvas.SetActive(false);
        }
    }
}
