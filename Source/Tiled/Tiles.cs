using System;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace Turnable.Tiled
{
    public class Tiles
    {
        readonly private MemoryStream _tiles = new MemoryStream();
        private BinaryReader _reader;

        public Tiles(Data data)
        {
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

        public uint At(uint col, uint row)
        {
            _tiles.Seek(0, SeekOrigin.Begin);
            return _reader.ReadUInt32();
        }
    }
}
