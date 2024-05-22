using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public sealed class BallLogger
    {
        public class BallLog
        {
            public int _id { get; set; }
            public DateTime _timestamp { get; set; }
            public double _x { get; set; }
            public double _y { get; set; }

            public BallLog(double x, double y, int id, DateTime timestamp)
            {
                _timestamp = timestamp;
                _x = x;
                _y = y;
                _id = id;
            }

            public override string ToString()
            {
                return $"{_timestamp:yyyy.MM.dd HH:mm:ss.fffffff} BallLog: Id = {_id}, X = {_x}, Y = {_y}";
            }

            public string ToJson()
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(this);
            }
        }

        private bool LOG_TO_FILE = true;
        private bool LOG_TO_CONSOLE = true;
        private string LOG_FILE_PATH = ".";
        private string LOG_FILE_NAME = "log.json";
        private static BallLogger _instance = null;
        private static readonly object _padlock = new object();

        private List<BallLog> loggedItems = new List<BallLog>();
        private readonly object logLock = new object();

        BallLogger()
        {
            if (LOG_TO_FILE || LOG_TO_CONSOLE)
            {
                if (LOG_TO_FILE)
                {
                    File.WriteAllText(Path.Combine(LOG_FILE_PATH, LOG_FILE_NAME), "[]");
                }

                Task.Run(() => LogTo());
                
            }

        }

        private void LogTo()
        {
            while (true)
            {
                BallLog oldestLog = null;
                lock (logLock)
                {
                    if (loggedItems.Count < 1)
                    {
                        Thread.Sleep(10);
                        continue;
                    }
                    oldestLog = loggedItems.OrderBy(log => log._timestamp).First();
                    loggedItems.Remove(oldestLog);
                }

                if (LOG_TO_FILE)
                {
                    var lines = File.ReadAllLines(Path.Combine(LOG_FILE_PATH, LOG_FILE_NAME)).ToList();
                    if (lines.Count == 1)
                    {
                        lines.RemoveAt(lines.Count - 1);
                        lines.Add("[");
                    }
                    else
                    {
                        lines.RemoveAt(lines.Count - 1);
                    }

                    if (lines.Count > 1)
                    {
                        lines[lines.Count - 1] += ",";
                    }

                    lines.Add(oldestLog.ToJson());
                    lines.Add("]");

                    File.WriteAllLines(Path.Combine(LOG_FILE_PATH, LOG_FILE_NAME), lines);
                }

                if (LOG_TO_CONSOLE)
                {
                    Console.WriteLine(oldestLog);
                }
            }
        }

        public static BallLogger Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BallLogger();
                    }
                    return _instance;
                }
            }
        }

        public void Log(BallLog ballLog)
        {
            lock (logLock)
            {
                loggedItems.Add(ballLog);
            }
        }

        public List<BallLog> GetLoggedItems()
        {
            lock (logLock)
            {
                return loggedItems.ToList();
            }
        }
    }
}