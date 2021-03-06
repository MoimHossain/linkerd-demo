Write-Host "Building Backend Container Image"
Write-Host "==========================================================="
docker build -t moimhossain/backend-linkerd-demo:latest -f .\Octolamp.BackendService\Dockerfile .

Write-Host "Building Frontend Container Image"
Write-Host "==========================================================="
docker build -t moimhossain/frontend-linkerd-demo:latest -f .\Octolamp.Frontend\Dockerfile .

Write-Host "Building Daemon Service Container Image"
Write-Host "==========================================================="
docker build -t moimhossain/daemon-linkerd-demo:$DockerImageTag -f ./Octolamp.DaemonService/Dockerfile .