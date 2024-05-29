using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using static Data.BallLogger;

namespace Data
{
    public static class BallLogger
    {
        private static readonly ConcurrentQueue<ICollision> _logMessages = new ConcurrentQueue<ICollision>();
        private static readonly string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.json");
        private static Thread _logThread;
        private static bool _isRunning = false;
        private static readonly object _lock = new object();
        private static bool LOG_TO_FILE = true;
        private static bool LOG_TO_CONSOLE = true;
        
        static BallLogger()
        {
            if (LOG_TO_FILE)
            {
                try
                {
                    File.WriteAllText(_logFilePath, "[]");

                }
                catch (Exception e)
                {
                    LOG_TO_FILE = false;
                    Console.WriteLine($"An error occurred during log file preparation: {e.Message}");
                    Console.WriteLine($"!!!Disabling log to file.");
                }

            }
        }

        public static void Log(ICollision collisionInfo)
        {
            Task.Run((() => LogTo(collisionInfo)));
        }
        
        public static void LogTo(ICollision collisionInfo)
        {

            if (LOG_TO_CONSOLE || LOG_TO_FILE)
            {
                _logMessages.Enqueue(collisionInfo);
            
                lock (_lock)
                {
                    if (!_isRunning)
                    {
                        _isRunning = true;
                        _logThread = new Thread(WriteLogTo);
                        _logThread.Start();
                    }
                }
            }
            
        }

        private static void WriteLogTo()
        {
            while (_logMessages.TryDequeue(out var logMessage))
            {
                
                if (LOG_TO_CONSOLE)
                {
                    Console.WriteLine($"{logMessage}\n");
                }

                if (LOG_TO_FILE)
                {
                     try
                     {
                         var lines = File.ReadAllText(_logFilePath);
                         var jsonArray = JsonConvert.DeserializeObject<List<string>>(lines); 
                         jsonArray.Add(logMessage.ToJson()); 
                         File.WriteAllText(_logFilePath, JsonConvert.SerializeObject(jsonArray));
                     }
                     catch (Exception e)
                     {
                         LOG_TO_FILE = false;
                         Console.WriteLine($"An error occurred log saving: {e.Message}");
                         Console.WriteLine($"!!!Disabling log to file.");
                     }

                }
            }

            _isRunning = false;
        }

        public interface ICollision
        {
            public string ToString();

            public string ToJson();
        }

        public class CollisionInfo : ICollision
        {
            public int Ball1Id { get; }
            public double Ball1X { get; }
            public double Ball1Y { get; }

            public int Ball2Id { get; }
            public double Ball2X { get; }
            public double Ball2Y { get; }

            public DateTime Timestamp { get; }

            public CollisionInfo(int ball1Id, double ball1X, double ball1Y, int ball2Id, double ball2X, double ball2Y)
            {
                Ball1Id = ball1Id;
                Ball1X = ball1X;
                Ball1Y = ball1Y;
                Ball2Id = ball2Id;
                Ball2X = ball2X;
                Ball2Y = ball2Y;
                Timestamp = DateTime.Now;
            }

            public override string ToString()
            {
                return $"Timestamp: {Timestamp:O}, Collision between Ball {Ball1Id} at ({Ball1X:F2}, {Ball1Y:F2}) and Ball {Ball2Id} at ({Ball2X:F2}, {Ball2Y:F2})";
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class CollisionInfoBoard : ICollision
        {
            public int Ball1Id { get; }
            public double Ball1X { get; }
            public double Ball1Y { get; }
            public string Wall { get; }

            public DateTime Timestamp { get; }

            public CollisionInfoBoard(int ball1Id, double ball1X, double ball1Y, string wall)
            {
                Ball1Id = ball1Id;
                Ball1X = ball1X;
                Ball1Y = ball1Y;
                Wall = wall;
                Timestamp = DateTime.Now;
            }

            public override string ToString()
            {
                return $"Timestamp: {Timestamp:O}, Collision between Ball {Ball1Id} at ({Ball1X:F2}, {Ball1Y:F2}) and wall {Wall:F2}";
            }

            public string ToJson()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
    }
}
