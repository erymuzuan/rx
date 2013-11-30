copy workflows.*.pdb .\bin\schedulers
copy workflows.*.pdb .\bin\subscribers

copy workflows.*.dll .\bin\schedulers
copy workflows.*.dll .\bin\subscribers
cd bin\subscribers
workers.console.runner.exe /log:console
cd ..\..\