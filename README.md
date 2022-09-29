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
