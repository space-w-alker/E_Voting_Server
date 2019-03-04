using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Election_Server.Security
{
    public class RSAImplementation
    {
        public static RSAParameters GlobalParameters;
        private static bool isParamNull = true;

        public static void CreateRSAInfo()
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSAParameters parameters = RSA.ExportParameters(true);
                using (FileStream file = File.OpenWrite("./Security/Secret.xml"))
                {
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(RSA.ToXmlString(true));
                    writer.Close();
                }
            }

        }

        public static RSAParameters LoadParameters()
        {
            if (isParamNull)
            {
                using (FileStream file = File.OpenRead("./Security/Secret.xml"))
                {
                    StreamReader reader = new StreamReader(file);
                    isParamNull = false;
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(reader.ReadToEnd());
                    GlobalParameters = rsa.ExportParameters(true);
                    return GlobalParameters;
                }
            }
            return GlobalParameters;

        }

        public static byte[] Decrypt(byte[] message, RSAParameters param)
        {
            byte[] encrypted;
            using (RSACryptoServiceProvider engine = new RSACryptoServiceProvider())
            {
                engine.ImportParameters(param);
                encrypted = engine.Decrypt(message, false);
            }
            return encrypted;
        }
    }
}
