using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Turnable.Tiled
{
    public class Tiles
    {
        private readonly MemoryStream _tiles = new MemoryStream();
        private readonly int _layerWidth;
        private readonly BinaryReader _reader;

        public Tiles(int layerWidth, Data data)
        {
            _layerWidth = layerWidth;

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
