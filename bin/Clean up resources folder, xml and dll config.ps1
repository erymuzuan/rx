ls -Filter Humanizer.resources.dll -Path .\ -Recurse | % { $_.FullName.Replace("Humanizer.resources.dll", "")} | Remove-Item -Force -Recurse
ls -Filter *.dll.config -Path .\bin\ -Recurse | Remove-Item
ls -Filter *.xml -Path .\ -Recurse | Remove-Item