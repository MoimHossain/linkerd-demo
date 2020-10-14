Write-Host "Building Backend Container Image"
Write-Host "==========================================================="
docker build -t moimhossain/stockdata:latest -f .\Octolamp.BackendService\Dockerfile .

Write-Host "Building Frontend Container Image"
Write-Host "==========================================================="
docker build -t moimhossain/stockweb:latest -f .\Octolamp.Frontend\Dockerfile .
