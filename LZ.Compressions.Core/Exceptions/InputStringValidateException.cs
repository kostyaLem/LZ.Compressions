using System;

namespace LZ.Compressions.Core.Exceptions
{
    public class InputStringValidateException : Exception
    {
        public InputStringValidateException()
        {
        }

        public InputStringValidateException(string message) : base(message)
        {
        }
    }
}
