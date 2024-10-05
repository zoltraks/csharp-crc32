using System.Security.Cryptography;

namespace Crc32
{
    public class Crc32 : HashAlgorithm
    {
        private uint checksum;

        private readonly uint[] table;

        public Crc32() : this(0xedb88320) { }

        public Crc32(uint polynomial)
        {
            HashSizeValue = 32;

            Initialize();

            table = new uint[256];

            for (uint i = 0; i < 256; i++)
            {
                uint next = i;
                for (uint j = 0; j < 8; j++)
                {
                    if ((next & 1) == 1)
                    {
                        next = (next >> 1) ^ polynomial;
                    }
                    else
                    {
                        next >>= 1;
                    }
                }
                table[i] = next;
            }
        }

        public override void Initialize()
        {
            checksum = 0xffffffff;
        }

        protected override void HashCore(byte[] buffer, int start, int length)
        {
            for (int i = start; i < length; i++)
            {
                byte next = buffer[i];
                byte index = (byte)((checksum & 0xff) ^ next);
                checksum = (checksum >> 8) ^ table[index];
            }
        }

        protected override byte[] HashFinal()
        {
            checksum = ~checksum;
            return BitConverter.GetBytes(checksum);
        }
    }
}
