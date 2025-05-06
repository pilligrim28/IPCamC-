using System;
using System.Diagnostics;
using System.IO;

namespace VideoSurveillanceApp.Services
{
    public class RecordingService
    {
        public void StartRecording(string rtspUrl, string cameraName)
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "recordings", cameraName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var filePath = Path.Combine(dir, $"recording_{DateTime.Now.Ticks}.mp4");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i {rtspUrl} -t 3600 -c:v copy -c:a aac {filePath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    // Обработка ошибки
                    Console.WriteLine($"Ошибка при записи: {process.StandardError.ReadToEnd()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение: {ex.Message}");
            }
        }

        public void CleanOldRecordings()
        {
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            if (!Directory.Exists(recordingsPath)) return;

            foreach (var folder in Directory.GetDirectories(recordingsPath))
            {
                var folderName = new DirectoryInfo(folder).Name;
                if (folderName == ".") continue;

                foreach (var file in Directory.GetFiles(folder))
                {
                    var age = DateTime.Now - File.GetLastWriteTime(file);
                    if (age.TotalDays > 7)
                        File.Delete(file);
                }
            }
        }
    }
}