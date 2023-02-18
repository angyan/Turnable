using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Turnable.TiledMap;

namespace Turnable.Tiled
{
    internal static class LayerData
    {
        //internal static Func<int, int, int> GetReaderFunc(string data, Encoding _encoding, Compression compression, int layerWidth)
        //{
        //    // Decode the data
        //    byte[] decodedData = Convert.FromBase64String(data);

        //    switch(compression)
        //    {
        //        case Compression.Zlib:
        //            MemoryStream compressedMemoryStream = new(decodedData);
        //            InflaterInputStream inflaterStream = new(compressedMemoryStream);
        //            MemoryStream tiles = new();
        //            inflaterStream.CopyTo(tiles);
        //            BinaryReader reader = new(tiles);

        //            return (col, row) =>
        //            {
        //                var seekLocation = (col + row * layerWidth) * 4;

        //                reader.BaseStream.Seek(seekLocation, SeekOrigin.Begin);
        //                return reader.ReadInt32();
        //            };
        //    }

        //    return (col, row) => 0;
        //}
    }
}
