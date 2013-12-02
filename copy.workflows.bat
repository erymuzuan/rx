copy bin\workflows.*.pdb .\bin\schedulers
copy bin\workflows.*.pdb .\bin\subscribers

copy bin\workflows.*.dll .\bin\schedulers
copy bin\workflows.*.dll .\bin\subscribers
cd bin\subscribers
workers.console.runner.exe /log:console /debug
cd ..\..\