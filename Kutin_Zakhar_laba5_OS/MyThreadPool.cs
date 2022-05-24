using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Kutin_Zakhar_laba5_OS
{
    public class MyThreadPool
    {
        private readonly ThreadPriority _Priority;
        private readonly Task[] _Threads;
        private readonly string? _Name;
        private readonly TaskStatus _Status;       
        
        ////очередь кортежей первый параметр - действие, которое надо выполнить, второй - параметр действия
        //private readonly Queue<(Action<object?> Work, object? Parameter)> _Works = new();

        //private readonly AutoResetEvent _WorkingEvent = new(false);
        //private readonly AutoResetEvent _ExecuteEvent = new(true);

        public MyThreadPool(int? MaxThreadsCount, ThreadPriority Priority = ThreadPriority.Normal, string? Name = null)
        {
            if (MaxThreadsCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxThreadsCount), MaxThreadsCount, "Число потоков в пуле должны быть больше 0");
            }
            _Priority = Priority;
            _Name = Name;
            Initialize();
        }

        private void Initialize()
        {
            //создаю общий канал данных
            Channel<string> channel0 = Channel.CreateBounded<string>(3);
            Channel<string> channel1 = Channel.CreateBounded<string>(3);
            Channel<string> channel2 = Channel.CreateBounded<string>(3);
            //создал токен отмены
            var cts0 = new CancellationTokenSource();
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            //создаются потоки
            _Threads[0] = Task.Run(() => { new TaskHash(0, channel0.Writer, channel0.Reader, "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad".ToUpper(), cts0.Token); }, cts0.Token);
            _Threads[1] = Task.Run(() => { new TaskHash(1, channel1.Writer, channel1.Reader, "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b".ToUpper(), cts1.Token); }, cts1.Token);
            _Threads[2] = Task.Run(() => { new TaskHash(2, channel2.Writer, channel2.Reader, "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f".ToUpper(), cts2.Token); }, cts2.Token);
            
            Task.WaitAll(_Threads);
        }

        private void ChangePriority(char modification)
        {

        }

        private void BlockAThread()
        {
            new Thread(() =>
            {
                if ()
                {

                }
                while (true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Q)
                    {
                        cts0.Cancel();
                        break;
                    }
                }

            }).Start();
        }

        private void ToPlanThreads()
        {

        }

        //public void Execute(Action Work) => Execute(null, _ => Work());

        //public void Execute(object? Parameter, Action<Object> Work)
        //{
        //    _ExecuteEvent.WaitOne(); // запрашиваем доступ к очереди
        //    _Works.Enqueue((Work, Parameter));
        //    _ExecuteEvent.Set(); // разрешили доступ к очереди

        //    _WorkingEvent.Set(); // разрешили работу потоку
        //}

        //private void WorkingThread()
        //{
        //    while (true)
        //    {
        //        _WorkingEvent.WaitOne();
        //        _ExecuteEvent.WaitOne(); // запрашиваем доступ к очереди
        //        var (work, parametr) = _Works.Dequeue();
        //        _ExecuteEvent.Set(); // разрешили доступ к очереди
        //        try
        //        {
        //            work(parametr);
        //        }
        //        catch (Exception e)
        //        {
        //            Trace.TraceError($"Ошибка выполнения задания в потоке {thread_name} : {e}");
        //        }
        //    }


        //}
    }
}
