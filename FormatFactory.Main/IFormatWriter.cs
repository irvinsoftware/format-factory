using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Irvin.FormatFactory
{
	public interface IFormatWriter
	{
		string WriteSingle<T>(T element, FormatOptions writerOptions = null);
		void WriteSingle<T>(StringBuilder container, T element, FormatOptions writerOptions = null);
		void WriteSingle<T>(Stream stream, T element, FormatOptions writerOptions = null);
		void WriteSingle<T>(TextWriter writer, T element, FormatOptions writerOptions = null);
		void WriteSingle<T>(string filename, T element, FormatOptions writerOptions = null);

		string Write<T>(IEnumerable<T> elements, FormatOptions writerOptions = null);
		void Write<T>(StringBuilder container, IList<T> elements, FormatOptions writerOptions = null);
		void Write<T>(Stream stream, IEnumerable<T> elements, FormatOptions writerOptions = null);
		void Write<T>(TextWriter writer, IEnumerable<T> elements, FormatOptions writerOptions = null);
		void Write<T>(string filename, IEnumerable<T> elements, FormatOptions writerOptions = null);

        string Write(DataTable dataTable, FormatOptions writerOptions);
        void Write(StringBuilder container, DataTable dataTable, FormatOptions writerOptions);
        void Write(Stream stream, DataTable dataTable, FormatOptions writerOptions);
        void Write(TextWriter writer, DataTable dataTable, FormatOptions writerOptions);
        void Write(string filename, DataTable dataTable, FormatOptions writerOptions);
	}
}