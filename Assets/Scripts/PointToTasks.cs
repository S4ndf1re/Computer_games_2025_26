using System.Collections.Generic;
using UnityEngine;

public class PointToConditions : MonoBehaviour
{


    public GameObject arrow;
    public Transform player;
    public float xLimit = 0f;
    public List<GameObject> conditions;
    public List<(TaskCondition, GameObject)> conditionsInner = new List<(TaskCondition, GameObject)>();
    public Transform currentTarget = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var cond in conditions)
        {
            var taskCond = cond.GetComponent<TaskCondition>();
            if (taskCond != null)
            {
                conditionsInner.Add((taskCond, cond));
            }
        }

    }

    Transform SelectCurrentTransform()
    {
        Transform newTarget = null;
        var minDist = float.MaxValue;

        foreach (var (possibleTarget, obj) in conditionsInner)
        {
            var dist = Vector3.Distance(obj.transform.position, player.position);
            if (!possibleTarget.TaskFinished() && dist < minDist && obj.transform.position.x <= xLimit)
            {
                newTarget = obj.transform;
                minDist = dist;
            }
        }

        return newTarget;
    }

    // Update is called once per frame
    void Update()
    {
        currentTarget = SelectCurrentTransform();

        if (currentTarget == null)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            transform.LookAt(currentTarget, Vector3.up);
        }


    }
}
