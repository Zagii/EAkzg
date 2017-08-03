
rem    Copy the required files to a convenient location.
rem    You can find the files needed in your Visual Studio project rem folder: ..\MyAddin\MyAddin\bin\Release
rem    Register your add-in dll in the COM codebase entries in the rem registry using regasm.exe
rem    Open up a command prompt in folder where you copied the add-rem in dll and register the dll with the /codebase option. In my 
rem    case that command would be: %WINDIR%\Microsoft.NET\Frameworkrem \v4.0.30319\regasm MyAddin.dll /codebase
rem    Add the registry key
rem    The easiest way to add the registry key on another computer rem is to export the key from your registry using regedit. This will 
rem    save the information stored in the key in a .reg file, which you rem can execute by doubleclicking.

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\regasm "D:\_Projekty\EAkzg\EAkzg\EAkzg\bin\Debug\EAkzg.dll" /codebase

pause.

Teraz trzeba w regedit dodac w \HKEY_CURRENT_USER\Software\Sparx Systems\EAAddins\EAkzg
i w domyœlnym kluczu zmieniæ wartoœæ na EAkzg.KzgAddinClass

pause.