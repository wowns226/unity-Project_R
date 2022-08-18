using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void OnTriggerEnter( Collider other )
    {
        if ( other.gameObject.CompareTag("Player") )
        {
            Debug.Log($"{this.gameObject.name} Enter");
            SceneManager.Instance.go_stageScene.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ( other.gameObject.CompareTag("Player") )
        {
            if ( Input.GetKeyDown(KeyCode.Space) ) 
            {
                SceneManager.Instance.go_stageScene.SetActive(true);
            }
        }
    }


    private void OnTriggerExit( Collider other )
    {
        if ( other.gameObject.CompareTag("Player") )
        {
            Debug.Log($"{this.gameObject.name} Exit");
            SceneManager.Instance.go_stageScene.SetActive(false);
        }
    }
}
