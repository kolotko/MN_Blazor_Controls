cd ..\MN_TextInput_Control
dotnet publish -c Release --self-contained false
dotnet pack -c Release MN_TextInput_Control.csproj -o .
dotnet nuget push *.nupkg --source "github"
DEL *.nupkg
pause