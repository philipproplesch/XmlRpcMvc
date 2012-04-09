$base = (Get-Item ..).FullName
$tmp = Join-Path $base 'tmp'
$out = Join-Path $base 'out'
$tls = Join-Path $base 'tls'

Task Default -depends Create-VsixArchives

Task Create-NuGetPackages -depends Build {

}

Task Build {
  
}