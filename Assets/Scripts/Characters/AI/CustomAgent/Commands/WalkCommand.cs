using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.AI.CustomAgent.Commands
{
    public sealed class WalkCommand<T> : IAgentCommand where T : struct
    {
        private readonly GameObject _agentObject;
        private readonly AgentMovement _agentMovement;
        private readonly T _startingPosition;
        public PathBuilder<T> PathBuilder { get; }

        public WalkCommand(GameObject agentObject, AgentMovement agentMovement, PathBuilder<T> pathBuilder, T startingPosition)
        {
            _agentObject = agentObject;
            _agentMovement = agentMovement;
            PathBuilder = pathBuilder;
            _startingPosition = startingPosition;
        }
        
        public void Execute()
        {
            var path = PathBuilder
                .BuildPath(_startingPosition, 7)
                .Select(t => t.Item1)
                .ToList();
            
            _agentMovement.RunCoroutine(_agentObject.transform, path);
        }
    }
}