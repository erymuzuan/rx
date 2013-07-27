$worker = ".\source\subscribers\workers.console.runner\bin\Debug\workers.console.runner.exe"
IF(Test-Path $worker){

    Write-Host "Starting worker..."
    Start-Process -FilePath $worker -WorkingDirectory ".\source\subscribers\workers.console.runner\bin\Debug\" -ArgumentList "/log:console"  -NoNewWindow
}
ELSE{
    Write-Host "Please compile your console.runner"
}