ls -Filter *.dll.config -Path .\bin -Recurse | Remove-Item
ls -Filter *.xml -Path .\subscribers -Recurse | Remove-Item
ls -Filter *.xml -Path .\control.center -Recurse | Remove-Item
ls -Filter *.xml -Path .\schedulers -Recurse | Remove-Item
ls -Filter *.xml -Path .\web -Recurse | Remove-Item
ls -Filter *.xml -Path .\tools -Recurse | Remove-Item
ls -Filter *.xml -Path .\subscribers.host -Recurse | Remove-Item
ls -Filter Humanizer.resources.dll -Path .\ -Recurse | % { $_.FullName.Replace("\Humanizer.resources.dll", "")} | Remove-Item -Force -Recurse
ls -Filter System.Spatial.resources.dll -Path .\ -Recurse | % { $_.FullName.Replace("\System.Spatial.resources.dll", "")} | Remove-Item -Force -Recurse
