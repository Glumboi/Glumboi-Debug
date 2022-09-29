# Glumboi-Debug
A small DLL I created to help debugging winforms projects, It's not complete yet.
Currently it only has a small console log that can be customized.

# How to use
To create a new instance of the debug console use this line: 
   
    public static DebugConsole debugConsole = new DebugConsole(2, "Debug Console", true, true);
    
You can also just use:

     public static DebugConsole debugConsole = new DebugConsole();

This will create a new debug console with the default params.
I personally implement this console in my Program.cs becasuse it contains the entry point of my programs.

After creatin you can write to it like so:
       
    Program.debugConsole.Info("Hello World!");
    Program.debugConsole.Warn("Warning!");
    Program.debugConsole.Error("Error!");
    
You can also change the message logging level like so: 
                                       
                                     an integer bewteen 0 and 2 (0 = error level, 1 = warning level, 2 = trace level)
    Program.debugConsole.ChangeLevel(↑);
    
I also use it often to get what it wrote to the console and save it in a text file, you can do it like so:
    
                                 bool : Append
    Program.debugConsole.SaveLog(↑, "./Log.txt");
