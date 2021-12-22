using System;

namespace Irvin.FormatFactory
{
    /// <summary>
    /// Thrown when attempting to read or write invalid data.
    /// </summary>
	public class InvalidDataException : Exception
	{
        /// <summary> 
        /// </summary>
        /// <param name="message">An explanation of the invalid data.</param>
		public InvalidDataException(string message) 
            : base(message)
		{
		}

        /// <summary> 
        /// </summary>
        /// <param name="message">An explanation of the invalid data.</param>
        /// <param name="innerException">The original exception the invalid data raised.</param>
        public InvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        } 
	}
}