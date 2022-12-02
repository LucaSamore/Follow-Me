using Characters.AI.CustomAgent.Commands;
using System.Collections.Generic;
using Characters.AI.Algorithms;
using UnityEngine;

namespace Characters.AI.CustomAgent
{
    public sealed class Agent<T> : IAgent where T : struct
    {
        public IAgentCommand WalkCommand { get; }
        public IAgentCommand JumpCommand { get; }

        public Agent(IDictionary<Vector3,T> navmesh, IPathStrategy<T> defaultAlgorithm)
        {
            WalkCommand = new WalkCommand<T>(new PathBuilder<T>(navmesh, defaultAlgorithm));
            JumpCommand = new JumpCommand();
        }

        public Agent(Agent<T> agent)
        {
            WalkCommand = agent.WalkCommand;
            JumpCommand = agent.JumpCommand;
        }

        public IAgent Clone() => new Agent<T>(this);
    }
}