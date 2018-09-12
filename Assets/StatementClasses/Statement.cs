using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class holds the metadata for any natural language statement we want the agent to
 *  process. When the user inputs a natural language statement to the agent, we match the statement
 *  based on the parts of speech that are present in the sentence and the order that they appear. When
 *  a statement is matched we run the associated sql command to return the desired data we need to successfully
 *  respond to the statment. 
 * 
 *  For example if the user tells the agent "go to the kitchen", the statementKey would be "go to" and the 
 *  statementComponent list would include a location. We would then run a sql command to get the location coordinates
 *  of the location so that we could then pass the coordinates to the navigation component. 
**/
public class Statement{

	public string statementKey; // The word or phrase we use to match the statement (ie 'pick up' or'what') 
	public List<WordType> statementComponents; // The types of words we expect to be found in the statment
	public string sqlCommand; // The sql command we want to be run when we make a match with this statment.

	public Statement(string statementKey, List<WordType> statementParts, string sqlText) {
		this.statementKey = statementKey;
		this.statementComponents = statementParts;
		this.sqlCommand = sqlText;
	}
		
}
