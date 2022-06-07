using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        // priority = 1
        // true here means we remove "isWaiting" from being a goal
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        // priority = 4
        goals.Add(s1, 3);      
    }
}
