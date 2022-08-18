using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDetail : MonoBehaviour
{
    private void OnDisable()
    {
        QuestManager questManager = QuestManager.Instance;
        questManager.Clear();
    }
}
