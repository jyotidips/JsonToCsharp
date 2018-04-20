using System;
using System.IO;

namespace JsonToCsharp
{

    interface ICharReader : IDisposable
    {
        int CurrentLine { get; }
        int CurrentLineOffset { get; }
        char ReadChar();
    }

    class FileReader : ICharReader
    {
        private readonly string _path;
        private readonly char[] _buffer = new char[1024];
        private int _bufferLength = 0;
        private readonly StreamReader _reader;
        private int _offset = 0;

        public int CurrentLine { get; private set; }
        public int CurrentLineOffset { get; private set; }

        internal FileReader(string path)
        {
            _path = path;
            _reader = File.OpenText(path);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public char ReadChar()
        {
            if (_offset >= _bufferLength)
            {
                _bufferLength = _reader.ReadBlock(_buffer, 0, _buffer.Length);
                if (_bufferLength <= 0)
                {
                    return '\0';
                }
                _offset = 0;
            }

            var result = _buffer[_offset++];
            if (result == '\n')
            {
                CurrentLine++;
                CurrentLineOffset = 0;
            }
            CurrentLineOffset++;
            return result;
        }
    }
}