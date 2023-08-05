cd ..\MN_Search_Control
dotnet publish -c Release --self-contained false
dotnet pack -c Release MN_Search_Control.csproj -o .
dotnet nuget push *.nupkg --source "github"
DEL *.nupkg
pause