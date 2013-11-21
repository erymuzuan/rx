copy workflows.*.pdb .\source\scheduler\scheduler.delayactivity\bin\Debug
copy workflows.*.pdb .\source\subscribers\workers.console.runner\bin\Debug

copy workflows.*.dll .\source\scheduler\scheduler.delayactivity\bin\Debug
copy workflows.*.dll .\source\subscribers\workers.console.runner\bin\Debug
cd source\subscribers\workers.console.runner\bin\Debug\
workers.console.runner.exe /log:console
cd ..\..\..\..\