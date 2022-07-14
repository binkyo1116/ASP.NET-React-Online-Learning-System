using System;

namespace CompilerFunctions
{
    public class SolutionNotFoundException : Exception
    {
        public SolutionNotFoundException(string msg) : base(msg)
        {

        }
    }
}