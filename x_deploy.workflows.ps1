copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\schedulers

copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\subscribers

copy bin\output\workflows.*.pdb .\bin\schedulers
copy bin\output\workflows.*.pdb .\bin\subscribers
copy bin\output\workflows.*.pdb .\source\web\web.sph\bin

copy bin\output\workflows.*.dll .\bin\schedulers
copy bin\output\workflows.*.dll .\bin\subscribers
copy bin\output\workflows.*.dll .\source\web\web.sph\bin

cd bin\subscribers
.\workers.console.runner.exe /log:console /debug
cd ..\..\