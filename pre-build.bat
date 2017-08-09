@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   GitVersion /output buildserver
)

REM Package restore
call %NuGet% restore Library\packages.config -OutputDirectory %cd%\packages -NonInteractive

REM Build
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" MVC5HtmlTable.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

REM Package
mkdir Build
call %nuget% pack "Library\MVC5HtmlTable.csproj" -symbols -o Build -p Configuration=%config% %version%