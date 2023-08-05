cd ..\MN_Checkbox_Control
dotnet publish -c Release --self-contained false
dotnet pack -c Release MN_Checkbox_Control.csproj -o .
dotnet nuget push *.nupkg --source "github"
DEL *.nupkg
pause