# 🔧 Update with your actual project root folder
$projectRoot = "C:\Users\giris\Documents\Code\Bar\Order Tracker\RestockMateApp"

# Define folder structure and required files
$structure = @{
    "Forms\Form1" = @(
        "Constructor.cs",
        "Events.cs",
        "Order.cs",
        "Inventory.cs",
        "Messaging.cs",
        "UserManagement.cs",
        "History.cs",
        "TabControl.cs"
    )
    "Forms\Form1\Designer" = @(
        "Form1.Designer.cs",
        "FormProperties.Designer.cs",
        "OrderTab.Designer.cs",
        "InventoryTab.Designer.cs",
        "UserTab.Designer.cs",
        "HistoryTab.Designer.cs",
        "TabControl.Designer.cs"
    )
    "Models" = @(
        "ItemDto.cs",
        "OrderDto.cs",
        "UserDto.cs",
        "StatusUpdateDto.cs"
    )
    "Services" = @(
        "WhatsAppMessenger.cs"
    )
}

# Create folders and files
foreach ($folder in $structure.Keys) {
    $fullFolderPath = Join-Path $projectRoot $folder
    if (!(Test-Path $fullFolderPath)) {
        New-Item -Path $fullFolderPath -ItemType Directory -Force | Out-Null
        Write-Host "📁 Created folder: $folder"
    }

    foreach ($file in $structure[$folder]) {
        $filePath = Join-Path $fullFolderPath $file
        if (!(Test-Path $filePath)) {
            New-Item -Path $filePath -ItemType File -Force | Out-Null

            # Add placeholder content
            $namespace = $folder -replace '\\','.' -replace '^Forms\.','RestockMateApp.Forms.' -replace '^Models$','RestockMateApp.Models' -replace '^Services$','RestockMateApp.Services'
            Add-Content -Path $filePath -Value "// Auto-created: $file`nnamespace $namespace { }"
            Write-Host "✅ Created file: $file in $folder"
        } else {
            Write-Host "✔️ File exists: $file in $folder"
        }
    }
}