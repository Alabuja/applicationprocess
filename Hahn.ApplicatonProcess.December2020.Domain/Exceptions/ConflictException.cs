using System;

namespace Hahn.ApplicatonProcess.December2020.Domain.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
