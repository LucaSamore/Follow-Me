using System.Collections;
using Characters.AI.CustomAgent.Commands;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class Agent<T> : IAgent where T : struct
    {
        public IAgentCommand WalkCommand { get; set; }
        public IAgentCommand JumpCommand { get; set; }

        public Agent(
            Transform agentTransform,
            AgentMovement agentMovement,
            IDictionary<Vector3,T> navmesh, 
            IPathStrategy<T> defaultAlgorithm, 
            T startingPosition)
        {
            WalkCommand = new WalkCommand<T>(agentTransform, agentMovement, new PathBuilder<T>(navmesh, defaultAlgorithm), startingPosition);
            JumpCommand = new JumpCommand();
        }

        public Agent(Agent<T> agent)
        {
            WalkCommand = agent.WalkCommand;
            JumpCommand = agent.JumpCommand;
        }

        public void Walk() => WalkCommand.Execute();
        
        public void Jump() => JumpCommand.Execute();

        public IAgent Clone() => new Agent<T>(this);
    }
}