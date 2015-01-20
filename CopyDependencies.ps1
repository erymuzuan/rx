function Copy-ToBin{
[CmdletBinding()]
 param(
    [Parameter(Mandatory=$True,ValueFromPipeline=$True,ValueFromPipelinebyPropertyName=$True)]
    [System.IO.FileInfo[]] $sources
  )
  PROCESS {
      Foreach ($d in $sources) {

            Copy-Item $d.FullName -Destination .\source\web\web.sph\bin
            Copy-Item $d.FullName -Destination .\source\web\core.sph\bin
            Copy-Item $d.FullName -Destination .\bin\subscribers
            Copy-Item $d.FullName -Destination .\bin\tools
            Copy-Item $d.FullName -Destination .\bin\schedulers
            Copy-Item $d.FullName -Destination .\source\unit.test\durandaljs.compiler.test\bin\Debug
      }
    }
}


ls -Path .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse | Copy-ToBin
ls -Path .\packages\Microsoft.Composition.1.0.30\lib\portable-net45+win8+wp8+wpa81 -Filter *.dll -Recurse | Copy-ToBin
ls -Path .\packages\odp.net.managed.121.1.2\lib\net40 -Filter Oracle.*.dll |Copy-ToBin
ls .\packages\Microsoft.AspNet.Mvc.5.2.2\lib\net45\System.Web.Mvc.dll | Copy-ToBin
ls -Path .\source\dependencies -Filter *.dll -Recurse| Copy-ToBin