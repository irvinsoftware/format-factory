using System;

namespace Irvin.FormatFactory
{
    /// <summary>
    /// This exception is thrown when the record, field, sub-record, or child element setup is not correct.
    /// </summary>
	public class InvalidUsageException : Exception
	{
        /// <summary> 
        /// </summary>
        /// <param name="message"></param>
		public InvalidUsageException(string message) 
            : base(message)
		{
		}
	}
}