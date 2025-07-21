# 🔧 Set your project path
$projectPath = "C:\Users\giris\Documents\Code\Bar\Order Tracker\RestockMateApp"

# Scan all .cs files inside the folder recursively
Get-ChildItem -Path $projectPath -Recurse -Filter *.cs | ForEach-Object {
    $filePath = $_.FullName
    $fileName = $_.Name
    $lines = (Get-Content $filePath).Count
    $content = Get-Content $filePath

    $className = ($content | Select-String -Pattern "class\s+\w+" | Select-Object -First 1).Line
    $partialInfo = if ($className -match "partial") { "✅ partial" } else { "" }

    Write-Output "`n📄 $fileName"
    Write-Output "Class: $className $partialInfo"
    Write-Output "Lines: $lines"

    $methods = ($content | Select-String -Pattern " (private|public|protected)\s+(async\s+)?\w+[\w<>,]*\s+\w+\(")
    if ($methods.Count -gt 0) {
        Write-Output "Methods:"
        $methods | ForEach-Object { " - $($_.Line.Trim())" }
    } else {
        Write-Output "Methods: None detected"
    }
}