using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    public int curretnArmor = 0;
    public GameObject armor;
    public int backForce;
    public float shootdownTime = 0.5f;
    public GameObject[] armorList = new GameObject[10];
    // Start is called before the first frame update
    void Start()
    {
        armor.GetComponent<MeshRenderer>().enabled = false;
        armor.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.O)) {
            armor.GetComponent<MeshRenderer>().enabled = true;
            armor.GetComponent<BoxCollider>().enabled = true;
            GetComponent<ThirdPersonMovement>().enabled = false;
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(shootdownTime);
        armor.GetComponent<MeshRenderer>().enabled = false;
        armor.GetComponent<BoxCollider>().enabled = false;
        GetComponent<ThirdPersonMovement>().enabled = true;
    }
}
