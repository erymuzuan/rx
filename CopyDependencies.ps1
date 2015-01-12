function Copy-ToBin{
[CmdletBinding()]
 param(
    [Parameter(Mandatory=$True,ValueFromPipeline=$True,ValueFromPipelinebyPropertyName=$True)]
    [System.IO.FileInfo[]] $sources
  )
  PROCESS {
      Foreach ($d in $sources) {

            Copy-Item $d.FullName -Destination .\source\web\web.sph\bin
            Copy-Item $d.FullName -Destination .\bin\subscribers
            Copy-Item $d.FullName -Destination .\bin\tools
            Copy-Item $d.FullName -Destination .\bin\schedulers
      }
    }
}


ls -Path .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse | Copy-ToBin
ls -Path .\packages\Microsoft.Composition.1.0.27\lib\portable-net45+win8+wp8+wpa81 -Filter *.dll -Recurse | Copy-ToBin