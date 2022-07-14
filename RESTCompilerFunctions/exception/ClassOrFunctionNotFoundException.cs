using System;

namespace CompilerFunctions
{
    public class ClassOrFunctionNotFoundException : Exception
    {
        public ClassOrFunctionNotFoundException(string msg) : base(msg)
        {

        }
    }
}