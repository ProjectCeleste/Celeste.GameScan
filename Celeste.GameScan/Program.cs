using ProjectCeleste.GameFiles.GameScanner;
using ProjectCeleste.GameFiles.GameScanner.Models;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Celeste.GameScan
{
    public class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Option<string>(
                    "--game-dir",
                    description: "Path to the directory where Spartan.exe is located") { IsRequired = true },
                new Option<bool>(
                    "--is-steam",
                    "Enabled for installations that are launchable through Steam"),
                new Option<bool>(
                    "--verbose",
                    "Write more detailed log about progress"),
            };

            rootCommand.Handler = CommandHandler.Create<string, bool, bool>(async(gameDir, isSteam, verboseMode) =>
            {
                Console.WriteLine($"Starting game scan towards {gameDir}");

                var gameScannner = new GameScannerManager(gameDir, isSteam);

                Console.WriteLine("Fetching manifest data");
                await gameScannner.InitializeFromCelesteManifest();

                Console.WriteLine("Scanning game files");
                var progress = new Progress<ScanProgress>();
                var subProgress = new Progress<ScanSubProgress>();

                if (!verboseMode)
                {
                    subProgress.ProgressChanged += SubProgress_ProgressChanged;
                    progress.ProgressChanged += Progress_ProgressChanged;
                }

                if (!await gameScannner.ScanAndRepair(progress, subProgress))
                {
                    Console.WriteLine("All files ");
                    return 0;
                }
                else
                {
                    Console.WriteLine("Gamescan has completed");
                    return 0;
                }
            });

            return await rootCommand.InvokeAsync(args);
        }

        private static void Progress_ProgressChanged(object sender, ScanProgress e)
        {
            Console.WriteLine($"{e.File} ({e.Index}/{e.TotalIndex}):");
        }

        private static void SubProgress_ProgressChanged(object sender, ScanSubProgress e)
        {
            switch (e.Step)
            {
                case ScanSubProgressStep.Check:
                    Console.WriteLine("Verifying file checksum");
                    break;
                case ScanSubProgressStep.Download:
                    if (e.DownloadProgress != null)
                    {
                        if (e.DownloadProgress.Size == 0)
                        {
                            Console.WriteLine("Starting download");
                        }
                        else
                        {
                            var downloaded = BytesSizeExtension.FormatToBytesSizeThreeNonZeroDigits(e.DownloadProgress.SizeCompleted);
                            var leftToDownload = BytesSizeExtension.FormatToBytesSizeThreeNonZeroDigits(e.DownloadProgress.Size);

                            var downloadSpeed = double.IsInfinity(e.DownloadProgress.Speed) ?
                                string.Empty : $"({BytesSizeExtension.FormatToBytesSizeThreeNonZeroDigits(e.DownloadProgress.Speed)}/s)";

                            Console.WriteLine($"Downloading {downloaded}/{leftToDownload} {downloadSpeed}");
                        }
                    }
                    break;
                case ScanSubProgressStep.CheckDownload:
                    Console.WriteLine("Checking downloaded file");
                    break;
                case ScanSubProgressStep.ExtractDownload:
                    Console.WriteLine("Extracting");
                    break;
                case ScanSubProgressStep.CheckExtractDownload:
                    Console.WriteLine("Checking extracted file");
                    break;
                case ScanSubProgressStep.Finalize:
                    Console.WriteLine("Finalizing");
                    break;
            }
        }
    }
}
