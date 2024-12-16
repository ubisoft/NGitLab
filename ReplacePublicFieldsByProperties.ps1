$rootPath = [IO.Path]::Combine($PSScriptRoot, "NGitLab", "Models")

$filesToModify = Get-ChildItem -Path $rootPath

foreach ($file in $filesToModify)
{
    $filePath = Join-Path $rootPath $file
    $content = Get-Content $filePath
    $content `
        -replace '(public\s+(\w+(<\w+>)?(\[\])?\??)\s+\w+\s*);', '$1 { get; set; }' `
        -replace '(public\s+(\w+(<\w+>)?(\[\])?\??)\s+\w+\s*)=', '$1{ get; set; } =' `
        | Out-File -Encoding utf8 $filePath
}