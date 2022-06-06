using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For Sorting Goals

public class SubGoal
{
    public Dictionary<string, int> subgoals;
    public bool remove; // once goal is satisfied, it is removed

    public SubGoal(string s, int i, bool r)
    {
        subgoals = new Dictionary<string, int>();
        subgoals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    // Start is called before the first frame update
    void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);     
    }

    void LateUpdate()
    {
        
    }
}
