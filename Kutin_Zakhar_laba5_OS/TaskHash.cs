using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Kutin_Zakhar_laba5_OS
{
    internal class TaskHash
    {
        private ChannelWriter<string> Writer;
        private ChannelReader<string> Reader;
        private int Id;
        
        private string PasswordHash;
        private static bool foundFlag;

        public TaskHash(int _id, ChannelWriter<string> _writer, ChannelReader<string> _reader, string _passwordHash, CancellationToken tok)
        {
            Id = _id;   
            Writer = _writer;
            Reader = _reader;
            PasswordHash = _passwordHash;
            Task.WaitAll(Run(tok));
        }
        private async Task Run(CancellationToken tok)
        {
            //ожидает, когда освободиться место для записи элемента.
            while (await Writer.WaitToWriteAsync())
            {
                if (tok.IsCancellationRequested)
                {
                    Console.WriteLine("\tПроизводитель остановлен.");
                    return;
                }

                char[] word = new char[5];
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
                                        await Writer.WriteAsync(new string(word));
                                        Console.WriteLine($"Работает {Id} поток");
                                    }
                                    else
                                    {
                                        Writer.Complete();
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // ожидает, когда освободиться место для чтения элемента.
            while (await Reader.WaitToReadAsync())
            {
                if (!foundFlag)
                {
                    var item = await Reader.ReadAsync();
                    //Console.WriteLine($"получены данные {item}");
                    if (FoundHash(item.ToString()) == PasswordHash)
                    {
                        Console.WriteLine($"\tПароль подобран - {item}");
                        foundFlag = true;
                    }
                }
                else return;

                //проверка токена
                if (tok.IsCancellationRequested)
                {
                    if (Reader.Count == 0)
                    {
                        Console.WriteLine("\tПотребитель остановлен. ");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Находит хеш str
        /// </summary>
        /// <param name="str"></param>
        /// <returns>хеш строки str</returns>
        static public string FoundHash(string str)
        {
            SHA256 sha256Hash = SHA256.Create();
            //Из строки в байтовый массив
            byte[] sourceBytes = Encoding.ASCII.GetBytes(str);
            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            return hash;
        }

    }
}
