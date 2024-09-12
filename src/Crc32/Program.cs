namespace Crc32
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string friendlyName = AppDomain.CurrentDomain.FriendlyName;

            if (args.Length != 1)
            {
                Console.WriteLine($"Usage: {friendlyName} <filename>");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }

            try
            {
                using FileStream fs = File.OpenRead(filePath);
                using var crc32 = new Crc32();
                byte[] hash = crc32.ComputeHash(fs);
                uint result = BitConverter.ToUInt32(hash, 0);
                Console.WriteLine(result.ToString("X8"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
