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
    public void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach (GAction a in acts)
            actions.Add(a);     
    }

    bool invoked = false;
    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
    void LateUpdate()
    {
        // Performing an action
        if (currentAction != null && currentAction.running)
        {
            // An agent has a goal && has reached that goal
            if (currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }       

        // Planning state
        if (planner == null | actionQueue == null)
        {
            planner = new GPlanner();

            // System.Linq
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                // null here is not the world states it's the agent's beliefs
                actionQueue = planner.plan(actions, sg.Key.subgoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }          
            }
        }
        //  Ran out of actions
        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }
            planner = null;
        }
        // Still has actions in Queue
        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();
            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }
                if (currentAction.target != null)
                {
                    currentAction.running = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                // Force a new plan. Although it may still offer you the same plan you failed earlier,
                // at least it ensures that the agent won't get stuck in the middle of a plan.
                actionQueue = null;                       
            }
        }
    }
}
