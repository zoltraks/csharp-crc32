using System.Security.Cryptography;

namespace Crc32
{
    public class Crc32 : HashAlgorithm
    {
        private uint crc;

        private readonly uint[] table;

        public override void Initialize()
        {
            crc = 0xffffffff;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            for (int i = start; i < length; i++)
            {
                byte next = buffer[i];
                byte index = (byte)((crc & 0xff) ^ next);
                crc = (crc >> 8) ^ table[index];
            }
        }

        protected override byte[] HashFinal()
        {
            crc = ~crc;
            return BitConverter.GetBytes(crc);
        }

        public Crc32() : this(0xedb88320) { }

        public Crc32(uint polynomial)
        {
            HashSizeValue = 32;

            Initialize();

            table = new uint[256];

            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (uint j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ polynomial;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                table[i] = crc;
            }
        }
    }
}
