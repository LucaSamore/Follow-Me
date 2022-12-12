using Characters.AI.CustomAgent.Commands;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using Map;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class Agent<T> : IAgent where T : struct
    {
        public GameObject AgentObject { get; set; }
        public IAgentCommand WalkCommand { get; set; }
        public IAgentCommand JumpCommand { get; set; }

        public Agent(
            GameObject agentObject,
            AgentMovement agentMovement,
            IDictionary<Vector3,T> navmesh, 
            IPathStrategy<T> defaultAlgorithm, 
            T startingPosition)
        {
            AgentObject = agentObject;
            WalkCommand = new WalkCommand<T>(agentObject, agentMovement, 
                new PathBuilder<T>(navmesh, defaultAlgorithm), startingPosition);
            JumpCommand = new JumpCommand();
        }

        public Agent(Agent<T> agent)
        {
            var prev = agent.AgentObject.GetComponent<AIController>().howMany;
            agent.AgentObject.GetComponent<AIController>().howMany = 0;
            agent.AgentObject.GetComponent<TileController>().color = 
                new Color(Random.Range(0f, 1f), Random.Range(0f, 1f),Random.Range(0f, 1f));
            AgentObject = GameObject.Instantiate(agent.AgentObject);
            agent.AgentObject.GetComponent<AIController>().howMany = prev;
            
            WalkCommand = agent.WalkCommand;
            JumpCommand = agent.JumpCommand;
        }

        public void Walk() => WalkCommand.Execute();
        
        public void Jump() => JumpCommand.Execute();

        public IAgent Clone() => new Agent<T>(this);
    }
}