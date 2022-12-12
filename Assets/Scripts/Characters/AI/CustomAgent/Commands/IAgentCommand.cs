namespace Characters.AI.CustomAgent.Commands
{
    /// <summary>
    /// This interface describes a command that can be executed by a custom agent.
    /// </summary>
    public interface IAgentCommand
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        void Execute();
    }
}