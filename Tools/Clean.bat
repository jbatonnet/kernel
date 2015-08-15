@echo off

:: Clean logs
forfiles /s /p ..\System /m *.tlog /c "cmd /c del /q @path"

:: Clean directories
forfiles /s /p ..\System /m obj /c "cmd /c rmdir /q /s @path" 2>nul
forfiles /s /p ..\System /m bin /c "cmd /c rmdir /q /s @path" 2>nul
forfiles /s /p ..\System /m Debug /c "cmd /c del /q /s @path\* 2>nul"
forfiles /s /p ..\System /m Debug /c "cmd /c attrib -s -h @path"
forfiles /s /p ..\System /m Debug /c "cmd /c attrib +s +h @path"
forfiles /s /p ..\System /m Release /c "cmd /c del /q /s @path\* 2>nul"
forfiles /s /p ..\System /m Release /c "cmd /c attrib -s -h @path"
forfiles /s /p ..\System /m Release /c "cmd /c attrib +s +h @path"
forfiles /s /p ..\System /m x64 /c "cmd /c del /q /s @path\* 2>nul"
forfiles /s /p ..\System /m x64 /c "cmd /c attrib -s -h @path"
forfiles /s /p ..\System /m x64 /c "cmd /c attrib +s +h @path"
forfiles /s /p ..\System /m ARM /c "cmd /c del /q /s @path\* 2>nul"
forfiles /s /p ..\System /m ARM /c "cmd /c attrib -s -h @path"
forfiles /s /p ..\System /m ARM /c "cmd /c attrib +s +h @path"

:: Clean VMware
del /q /s ..\VMware\*.log 2>nul
del /q /s ..\VMware\*.dmp 2>nul