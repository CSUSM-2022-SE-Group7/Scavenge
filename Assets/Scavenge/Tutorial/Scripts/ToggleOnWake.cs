using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnWake : MonoBehaviour
{
    public GameObject ghost;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("awjidoaiwjodaw");
        ghost.SetActive(true);
    }

    private void OnDisable()
    {
        ghost.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
