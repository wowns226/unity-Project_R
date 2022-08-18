using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    private void Awake()
    {
        if ( Instance == null )
            Instance = this;
        else
            DontDestroyOnLoad(Instance);
    }

    public GameObject go_stageScene;
    public GameObject playerObj;
    public GameObject BattlePortal;
    public GameObject VillagePortal;

    public Vector3 TeleportToVillagePos;
    public Vector3 TeleportToBattlePos;
    

    public void onStartBtn()
    {
        LoadSceneCtrl.LoadScene("1VillageScene");
    }

    public void onExitBtn()
    {
        Application.Quit();
    }

    public void onClickVilliage()
    {
        Debug.Log("onClickVilliage");
        Player.instance.FieldIndex = -1;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;

        playerObj.transform.position = VillagePortal.transform.position + TeleportToVillagePos;

        playerObj.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void onClickStageSlime()
    {
        Debug.Log("onClickStageSlime");
        Player.instance.FieldIndex = 0;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;

        playerObj.transform.position = BattlePortal.transform.position + TeleportToBattlePos;

        playerObj.GetComponent<NavMeshAgent>().enabled = true;
    }
    public void onClickStageGrunt()
    {
        Debug.Log("onClickStageGrunt");
        Player.instance.FieldIndex = 1;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;

        playerObj.transform.position = BattlePortal.transform.position + TeleportToBattlePos;

        playerObj.GetComponent<NavMeshAgent>().enabled = true;
    }
    public void onClickStageLich()
    {
        Debug.Log("onClickStageLich");
        Player.instance.FieldIndex = 2;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;

        playerObj.transform.position = BattlePortal.transform.position + TeleportToBattlePos;
        
        playerObj.GetComponent<NavMeshAgent>().enabled = true;
    }
    public void onClickStageBoss()
    {
        Player.instance.FieldIndex = 3;
        playerObj.GetComponent<NavMeshAgent>().enabled = false;

        playerObj.transform.position = BattlePortal.transform.position + TeleportToBattlePos;

        playerObj.GetComponent<NavMeshAgent>().enabled = true;
    }
}
