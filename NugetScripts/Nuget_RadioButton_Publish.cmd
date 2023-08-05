cd ..\MN_RadioButton_Control
dotnet publish -c Release --self-contained false
dotnet pack -c Release MN_RadioButton_Control.csproj -o .
dotnet nuget push *.nupkg --source "github"
DEL *.nupkg
pause