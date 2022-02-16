using System;

namespace Festival.BL.Exceptions
{
    public class DateTimeCollisionException : Exception
    {
        public DateTimeCollisionException()
        {
        }

        public DateTimeCollisionException(string message)
            : base(message)
        {
        }

        public DateTimeCollisionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class DateTimeStartEndException : Exception
    {
        public DateTimeStartEndException()
        {
        }

        public DateTimeStartEndException(string message)
            : base(message)
        {
        }

        public DateTimeStartEndException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
