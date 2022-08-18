using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaBuiiding : MonoBehaviour
{
    List<GameObject> Alphalist = new List<GameObject>();
    List<GameObject> Recoverlist = new List<GameObject>();

    public GameObject player;


    // obj가 알파리스트에 포함되어 있는지 검사
    public GameObject FindAlphalist(GameObject obj)
    {
        GameObject findObj = Alphalist.Find(o => (o.name == obj.name));
        return findObj;
    }
    
    public void AddAlplalist(GameObject obj)
    {
        GameObject alphaObj = FindAlphalist(obj);

        if (alphaObj == null)
        {
            
            if (obj.CompareTag("Obtacle"))
            {
                Alphalist.Add(obj);
                Color col = obj.GetComponent<MeshRenderer>().material.color;
                col.a = 0.2f;
                obj.GetComponent<MeshRenderer>().material.color = col;

            }

        }
    }
    /*
    void Update()
    {
         Alphalist;
    }
    */
    private void LateUpdate()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = player.transform.position - Camera.main.transform.position;

        RaycastHit[] hits = Physics.RaycastAll(origin, dir.normalized);

        if (hits.Length == 0)
        {
            for (int i = 0; i < Alphalist.Count; i++)
            {
                Color col = Alphalist[i].GetComponent<MeshRenderer>().material.color;
                col.a = 1f;
                Alphalist[i].GetComponent<MeshRenderer>().material.color = col;
            }
            Alphalist.Clear();

            return;
        }
        // 광선과 충돌한 게임오브젝트 전체를 리스트에 저장
        for (int i = 0; i < hits.Length; i++)
        {
            AddAlplalist(hits[i].collider.gameObject);
        }

        // 복원구현
        // 알파리스트에서 빠져나간 경우
        for( int i = 0; i < Alphalist.Count; i++)
        {
            GameObject tmp = null;

            for (int j = 0; j < hits.Length; j++)
            {
                try
                {
                    if (Alphalist[i].name == hits[j].collider.gameObject.name)
                    {
                        tmp = hits[j].collider.gameObject;
                    }
                }
                catch( System.IndexOutOfRangeException)
                {
                    Debug.Log("i index = " + i);
                    Debug.Log("j index = " + j);
                }
            }

            if( tmp == null )
            {
                GameObject recoverObj = Recoverlist.Find(o => (o.name == Alphalist[i].name));
                if (recoverObj != null)
                    continue;

                Color col = Alphalist[i].GetComponent<MeshRenderer>().material.color;
                col.a = 1f;
                Alphalist[i].GetComponent<MeshRenderer>().material.color = col;
                //                Alphalist.Remove(Alphalist[i]);
                //                break;
                Recoverlist.Add(Alphalist[i]);
            }
        }

        // Recoverlist에 있는 오브젝트를 Alphalist에서 제거
        GameObject[] recoverArray = Recoverlist.ToArray();

        for (int i = 0; i < recoverArray.Length; i++)
        {
            try
            {
                GameObject findObj = Alphalist.Find(o => (o.name == Recoverlist[i].name));
                if (findObj != null)
                {
                    Alphalist.Remove(findObj);
                }
            }
            catch( System.ArgumentOutOfRangeException e)
            {
                Debug.Log("i index = " + i + e.ToString());
            }
        }

        Recoverlist.Clear();
    }
}
