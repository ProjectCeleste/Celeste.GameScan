# About
This is a small command line application that can be used for running game scans for Age of Empires Online Project Celeste from the command line interface on platforms that supports .NET Core.

# Usage
```
Usage:
  Celeste.GameScan [options]

Options:
  --game-dir <game-dir> (REQUIRED)    Path to the directory where Spartan.exe is located
  --is-steam                          Enabled for installations that are launchable through Steam
  --verbose                           Write more detailed log about progress
  --version                           Show version information
  -?, -h, --help                      Show help and usage information
```

# Example
Start a game scan in the directory `D:\AgeOfEmpiresOnline`
```
Celeste.GameScan.exe --game-dir D:\AgeOfEmpiresOnline
```

# Building and running
To build and run this you must have the .NET Core 3.1 SDK installed. To compile the program, run `dotnet build` or compile and run it directly with `dotnet run -- --game-dir "D:\Test"` 