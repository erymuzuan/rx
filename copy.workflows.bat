@echo off
copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\schedulers

copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\subscribers

copy bin\workflows.*.pdb .\bin\schedulers
copy bin\workflows.*.pdb .\bin\subscribers

copy bin\workflows.*.dll .\bin\schedulers
copy bin\workflows.*.dll .\bin\subscribers

cd bin\subscribers
workers.console.runner.exe /log:console /debug
cd ..\..\

@echo on