using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add the UXF namespace
using UXF;

public class ExperimentGenerator : MonoBehaviour{
	// Private
	private int numTrials;

    // generate the blocks and trials for the session.
    // the session is passed as an argument by the event call.
    public void Generate(Session session){
    	// Get trials from .json file
    	numTrials = session.settings.GetInt("numTrials");

        // generate a single block with x trials.
        session.CreateBlock(numTrials);
    }
}
