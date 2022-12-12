using UnityEngine;

namespace Characters.AI.CustomAgent
{
    /// <summary>
    /// An interface for describing a custom agent.
    /// </summary>
    public interface IAgent
    {
        /// <summary>
        /// Makes an agent copy.
        /// </summary>
        /// <returns>A deep copy of this agent.</returns>
        IAgent Clone();
        
        /// <summary>
        /// Executes a <see cref="Commands.IAgentCommand"/> in order to make the agent walk.
        /// </summary>
        void Walk();
        
        /// <summary>
        /// Executes a <see cref="Commands.IAgentCommand"/> in order to make the agent jump.
        /// </summary>
        void Jump();
        
        /// <summary>
        /// Represents the <c>GameObject</c> the agent is attached to.
        /// </summary>
        GameObject AgentObject { get; set; }
    }
}