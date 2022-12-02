﻿namespace Characters.AI.CustomAgent
{
    public interface IAgent
    {
        IAgent Clone();
        
        void Walk();
        
        void Jump();
    }
}