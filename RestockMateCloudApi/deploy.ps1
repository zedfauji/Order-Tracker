# 💥 Bypass script restrictions temporarily
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# 📁 Set project folder (adjust if needed)
$projectFolder = "RestockMateCloudApi"

# 🌍 Set your Google Cloud project ID
$projectId = "bola8pos"

# 📡 Set the Cloud Run service name
$serviceName = "restock-api"

# 🌐 Set the region
$region = "us-central1"

# 🧭 Authenticate with Google Cloud (assumes gcloud CLI is installed)
gcloud auth login
gcloud config set project $projectId

# 🔨 Build and deploy using Cloud Run (from local source)
gcloud run deploy $serviceName `
  --source $projectFolder `
  --region $region `
  --allow-unauthenticated

Write-Host "`n✅ Cloud Run deployment complete. Your API is live at:"
gcloud run services describe $serviceName --region $region --format='value(status.url)'