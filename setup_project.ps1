$projectName = "RestockMate"
$basePath = "$PWD\$projectName"

# Create folders
New-Item -ItemType Directory -Path "$basePath\src\RestockMateApp" -Force
New-Item -ItemType Directory -Path "$basePath\src\Resources" -Force
New-Item -ItemType Directory -Path "$basePath\Orders" -Force

# Create placeholder files
New-Item -ItemType File -Path "$basePath\src\RestockMateApp\Program.cs" -Force
New-Item -ItemType File -Path "$basePath\src\RestockMateApp\MainForm.cs" -Force
New-Item -ItemType File -Path "$basePath\src\RestockMateApp\MainForm.Designer.cs" -Force
New-Item -ItemType File -Path "$basePath\src\RestockMateApp\WhatsAppHelper.cs" -Force
New-Item -ItemType File -Path "$basePath\src\RestockMateApp\CSVHandler.cs" -Force
New-Item -ItemType File -Path "$basePath\src\Resources\items_template.csv" -Force
New-Item -ItemType File -Path "$basePath\Orders\order_history.csv" -Force
New-Item -ItemType File -Path "$basePath\README.md" -Force

# Create solution file (placeholder)
New-Item -ItemType File -Path "$basePath\$projectName.sln" -Force

Write-Host "✅ Project structure for '$projectName' created successfully at: $basePath"