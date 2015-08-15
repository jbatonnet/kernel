@echo off

echo -- Projet metrics --
powershell "\"\""

echo Files
powershell "\"  Sources: \" + (dir ..\System *.cpp -recurse).Count"
powershell "\"  Headers: \" + (dir ..\System *.h -recurse).Count"
powershell "\"\""

echo Lines
powershell "\"  Total: \" + ((dir ..\System -include *.cpp,*.h -recurse | select-string .).Count - (dir ..\System\Kernel\Libraries -include *.cpp,*.h -recurse | select-string .).Count)"
powershell "\"  Comments: \" + ((dir ..\System -include *.cpp,*.h -recurse | select-string \"//\").Count - (dir ..\System\Kernel\Libraries -include *.cpp,*.h -recurse | select-string \"//\").Count)"
powershell "\"  Brackets: \" + ((dir ..\System -include *.cpp,*.h -recurse | select-string \"[{}]\").Count - (dir ..\System\Kernel\Libraries -include *.cpp,*.h -recurse | select-string \"[{}]\").Count)"
powershell "\"\""

pause