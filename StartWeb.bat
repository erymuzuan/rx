copy .\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll
copy .\source\web\web.sph.commercial-space\bin\domain.commercialspace.pdb
copy .\source\web\web.sph.commercial-space\bin\roslyn.scriptengine.dll
copy .\source\web\web.sph.commercial-space\bin\roslyn.scriptengine.pdb
"C:\Program Files (x86)\IIS Express\iisexpress.exe" /config:%USERPROFILE%\Documents\IISExpress\config\applicationhost.config /site:web.sph.commercial-space /trace:verbose
