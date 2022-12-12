using Characters.AI.CustomAgent.Commands;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using Map;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    /// <summary>
    /// This class is a concrete implementation of <see cref="IAgent"/> interface.
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public sealed class Agent<T> : IAgent where T : struct
    {
        public GameObject AgentObject { get; set; }
        public IAgentCommand WalkCommand { get; set; }
        public IAgentCommand JumpCommand { get; set; }

        /// <summary>
        /// Creates a new custom agent.
        /// </summary>
        /// <param name="agentObject">The <c>GameObject</c> the agent is attached to.</param>
        /// <param name="agentMovement">A script for managing the agent movement.</param>
        /// <param name="navmesh">The custom NavMesh where the agent can walk.</param>
        /// <param name="defaultAlgorithm">The algorithm used to generate paths.</param>
        /// <param name="startingPosition">The agent initial position on the NavMesh.</param>
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
        
        /// <summary>
        /// Creates a new custom agent from an existing agent.
        /// </summary>
        /// <param name="agent">The agent to copy</param>
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

        /// <inheritdoc cref="IAgent.Walk"/>
        public void Walk() => WalkCommand.Execute();
        
        /// <inheritdoc cref="IAgent.Jump"/>
        public void Jump() => JumpCommand.Execute();

        /// <inheritdoc cref="IAgent.Clone"/>
        public IAgent Clone() => new Agent<T>(this);
    }
}