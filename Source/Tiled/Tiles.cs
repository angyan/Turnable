using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Turnable.Tiled
{
    public class Tiles
    {
        readonly private MemoryStream _tiles = new MemoryStream();
        readonly private int _layerWidth;
        readonly private int _layerHeight;
        private readonly BinaryReader _reader;

        public Tiles(int layerWidth, int layerHeight, Data data)
        {
            _layerWidth = layerWidth;
            _layerHeight = layerHeight;

            // Decode the data
            byte[] decodedData = Convert.FromBase64String(data.Value);

            // Uncompress the decoded contents
            switch (data.Compression)
            {
                case Compression.Zlib:
                    var compressedMemoryStream = new MemoryStream(decodedData);
                    var inflaterStream = new InflaterInputStream(compressedMemoryStream);

                    inflaterStream.CopyTo(_tiles);
                    _reader = new BinaryReader(_tiles);
 
                    break;
            }
        }

        public uint this[int col, int row]
        {
            get
            {
                var seekLocation = (col + row * _layerWidth) * 4;

                _tiles.Seek(seekLocation, SeekOrigin.Begin);
                return _reader.ReadUInt32();
            }
        }
    }
}
