using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace signalr_best_practice_core.Configuration
{
    public class Log : IDisposable
    {
        public static readonly Log _instance = new Log();
        public static Log Current => _instance;

        public event Action<ItemMessage> OnNewMessage;
        private FileInfo _fi;
        private ConcurrentQueue<ItemMessage> _messages = new ConcurrentQueue<ItemMessage>();

        public bool IsActive { get; private set; }
        private Thread _thread;

        public Log()
        {
            Start();
        }

        public void Error(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Error, memberName);
        }

        public void Error(Exception e, [CallerMemberName] string memberName = "")
        {
            if (e == null) return;

            StringBuilder sb = new StringBuilder();
            var exception = e;
            int counter = 7;

            do
            {
                sb.Append(exception.ToString() + "\r\n");
                exception = exception.InnerException;

            } while (exception != null && exception.InnerException != exception && --counter > 0);

            ChangeProperties(sb.ToString(), TypeMessage.Error, memberName);
        }

        public void Message(string text, [CallerMemberName]string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Message, memberName);
        }

        public void TestMessage(string text, [CallerMemberName] string memberName = "")
        {
#if DEBUG
            Trace.WriteLine("Test: " + text);
            ChangeProperties(text, TypeMessage.TestMessage, memberName);
#endif
        }

        public void MultyMessage(Dictionary<string, string> pairs, [CallerMemberName] string memberName = "")
        {
            var text = pairs.Aggregate(string.Empty, (current, pair) => current + "\r\n" + pair.Key + ": " + pair.Value);
            ChangeProperties(text.Trim(), TypeMessage.Message, memberName);
        }

        public void Request(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Request, memberName);
        }

        public void Warning(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Warning, memberName);
        }

        public void Success(string text, [CallerMemberName] string memberName = "")
        {
            ChangeProperties(text, TypeMessage.Success, memberName);
        }

        private bool GetPathToTextLog()
        {
            if (_fi == null || !_fi.Exists || _fi.Length > 1024 * 1024 * 50)
            {
                var str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(str)) Directory.CreateDirectory(str);
                _fi = new FileInfo(Path.Combine(str, DateTime.UtcNow.ToString("dd.MM.yyyy_HH") + "-00.log"));
                return true;
            }
            return false;
        }

        private void ChangeProperties(string text, TypeMessage typeMessage, string memberName)
        {
            var userName = Thread.CurrentPrincipal?.Identity?.Name;
            var message = new ItemMessage
            {
                Message = text,
                MemberName = memberName,
                Type = typeMessage,
                NameUser = userName,
                Time = DateTime.UtcNow
            };

            _messages.Enqueue(message);
        }

        private void Process()
        {
            while (IsActive)
            {
                try
                {
                    ItemMessage message;

                    if (_messages.TryDequeue(out message))
                    {
                        var isNew = FileMode.Append;
                        if (GetPathToTextLog()) isNew = FileMode.Create;

                        OnNewMessage?.Invoke(message);
                        Debug.WriteLine(message.ToString());

                        using (var writer = new StreamWriter(new FileStream(_fi.FullName, isNew)))
                        {
                            writer.WriteLine(message.ToString());
                        }
                        continue;
                    }
                    Thread.Sleep(500);
                }
                catch (Exception)
                {

                }
            }
        }

        private void Start()
        {
            if (IsActive) return;
            IsActive = true;

            _thread = new Thread(Process);
            _thread.IsBackground = true;
            _thread.Start();
        }

        private void Stop()
        {
            IsActive = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }


    public struct ItemMessage
    {
        public DateTime Time;
        public TypeMessage Type;
        public string Message;
        public string NameUser;
        public string MemberName { get; set; }

        public override string ToString()
        {
            return $"{Time:dd.MM.yyyy HH:mm:ss.ffff} <{NameUser}> [{Type.ToString().ToUpperInvariant()}][{MemberName}] {Message}";
        }
    }

    public enum TypeMessage
    {
        Default = 0,
        Proxy = 1,
        Message = 2,
        Warning = 3,
        Error = 4,
        Request = 5,
        Success = 6,
        TestMessage = 7
    }
}
