using System;

namespace ArchitectProject.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException()
        {

        }

        public InvalidRequestException(string message)
            : base(message)
        {

        }
    }
}
