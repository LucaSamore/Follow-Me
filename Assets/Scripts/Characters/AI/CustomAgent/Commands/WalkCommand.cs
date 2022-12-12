using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.AI.CustomAgent.Commands
{
    /// <summary>
    /// Moves a custom agent along a generated path.
    /// This class implements the <see cref="IAgentCommand"/> interface and uses a <see cref="PathBuilder"/> to generate the path.
    /// The movement is handled by a <see cref="AgentMovement"/> script.
    /// </summary>
    /// <typeparam name="T">Any struct that represents a position in a custom coordinate system.</typeparam>
    public sealed class WalkCommand<T> : IAgentCommand where T : struct
    {
        private readonly GameObject _agentObject;
        private readonly AgentMovement _agentMovement;
        private readonly T _startingPosition;

        public PathBuilder<T> PathBuilder { get; }
        
        /// <summary>
        /// Creates a new <c>WalkCommand</c> instance.
        /// </summary>
        /// <param name="agentObject">The agent <c>GameObject</c>.</param>
        /// <param name="agentMovement">A script for managing the agent movement.</param>
        /// <param name="pathBuilder">A <c>PathBuilder</c> instance for path generation.</param>
        /// <param name="startingPosition">The source of the path to be generated.</param>
        public WalkCommand(GameObject agentObject, AgentMovement agentMovement, PathBuilder<T> pathBuilder, T startingPosition)
        {
            _agentObject = agentObject;
            _agentMovement = agentMovement;
            PathBuilder = pathBuilder;
            _startingPosition = startingPosition;
        }
        
        /// <inheritdoc cref="IAgentCommand.Execute"/>
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