using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Irvin.FormatFactory
{
    public interface IFormatReader
    {
        T ReadSingle<T>(string content, FormatOptions readOptions);
        T ReadSingle<T>(Stream stream, FormatOptions readOptions = null);
        List<T> Read<T>(string input, FormatOptions readOptions = null);
        DataTable ReadTable(string content, FormatOptions readOptions);
        List<T> ReadFromFile<T>(string filename, FormatOptions readOptions = null);
    }
}