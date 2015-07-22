Param(
       [string]$Entity = 10
     )
csc .\bin\sources\_generated\$Entity\*.cs /r:.\source\web\web.sph\bin\System.Web.Mvc.dll /r:.\source\web\web.sph\bin\domain.sph.dll /r:.\source\web\web.sph\bin\core.sph.dll /t:library /r:System.Net.Http.dll /out:.\bin\output\DevV1.$Entity.dll /pdb:.\bin\output\DevV1.$Entity.pdb /debug