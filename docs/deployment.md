# Deployment Guide - Google Cloud Run

## Environment Variables

Required environment variables for production:

```env
NODE_ENV=production
PORT=3000
FIREBASE_PROJECT_ID=your-project-id
FIREBASE_SERVICE_ACCOUNT_KEY={"type":"service_account",...}
JWT_SECRET=your-jwt-secret
JWT_REFRESH_SECRET=your-refresh-secret
MAPBOX_TOKEN=your-mapbox-token
CORS_ORIGIN=https://your-frontend-domain.com
```

## Google Cloud Run Setup

1. Enable required APIs:
```bash
gcloud services enable run.googleapis.com
gcloud services enable cloudbuild.googleapis.com
gcloud services enable containerregistry.googleapis.com
```

2. Build and deploy:
```bash
gcloud builds submit --config cloudbuild.yaml
```

## Auto-Scaling Strategy

- **Min instances**: 1 (always warm)
- **Max instances**: 100 (peak load)
- **CPU**: 1 vCPU
- **Memory**: 512Mi
- **Concurrency**: 80 requests per instance
- **Scaling target**: 80% CPU utilization

## Monitoring Setup

### Google Cloud Monitoring

1. Create uptime check for health endpoint
2. Setup alerting policies:
   - Error rate > 5%
   - Latency > 500ms
   - CPU > 80% for 5 minutes
   - Memory > 80% for 5 minutes

### Logs

- Cloud Logging automatically enabled
- Filter logs by severity
- Setup log-based metrics

## Database Backup Strategy

### Firebase Firestore

- **Automatic backups**: Enabled via Firebase Console
- **Retention**: 30 days
- **Export schedule**: Daily at 2 AM UTC
- **Export to**: Google Cloud Storage

### Manual Backup

```bash
gcloud firestore export gs://your-backup-bucket
```

## CI/CD Pipeline

GitHub Actions automatically:
1. Runs tests on push
2. Builds Docker image
3. Pushes to GCR
4. Deploys to Cloud Run

## Security

- HTTPS enabled by default
- VPC-Connector for private Firebase access
- IAM roles: Cloud Run Admin, Service Account User
