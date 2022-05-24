using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kutin_Zakhar_laba5_OS
{
    public class Menu
    {
        private static bool foundFlag = false;

        static public bool FoundHash(string PasswordHash)
        {
            char[] word = new char[5];
            while (!foundFlag)
            {
                for (int i = 97; i < 123; i++)
                {
                    word[0] = (char)i;
                    for (int k = 97; k < 123; k++)
                    {
                        word[1] = (char)k;
                        for (int l = 97; l < 123; l++)
                        {
                            word[2] = (char)l;
                            for (int m = 97; m < 123; m++)
                            {
                                word[3] = (char)m;
                                for (int n = 97; n < 123; n++)
                                {
                                    word[4] = (char)n;
                                    if (!foundFlag)
                                    {
                                        string item = new string(word);
                                        SHA256 sha256Hash = SHA256.Create();
                                        //Из строки в байтовый массив
                                        byte[] sourceBytes = Encoding.ASCII.GetBytes(item);

                                        byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                                        string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                                        if (hash == "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad".ToUpper())
                                        {
                                            Console.WriteLine($"\tПароль подобран - {item}");
                                            foundFlag = true;

                                        }
                                    }
                                }

                            }

                        }

                    }

                }
            }
            return foundFlag;
        }
        static public void Creatе()
        {

        }

        static public void PrintMenu()
        {
            Console.WriteLine("Программа запущена");
            
            
            using (MyThreadPool pool = new MyThreadPool(3))
            {
                pool.Execute(() => FoundHash("1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad".ToUpper()));
            }

        }
    }
}
