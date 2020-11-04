#!/bin/bash
DockerImageTag=$1
ResourceGroup=$2
SignalRName=$3
DockerPassword=$4

echo " "
echo "Preparing manifests with build Tag $DockerImageTag"

echo "SignalR Information:"
echo "Resoure Group: $ResourceGroup"
echo "Name: $SignalRName"

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "┊ Creating SignalR                                   │"
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"

az signalr create --name $SignalRName --resource-group $ResourceGroup --sku Standard_S1 --unit-count 1 --service-mode Default
primaryConnectionString=$(az signalr key list --name $SignalRName --resource-group $ResourceGroup --query primaryConnectionString -o tsv)


echo "Connection String retrieved.. replacing...$primaryConnectionString"
sed -i "s|SIGNALRPCINFO|$primaryConnectionString|" backend-linkerd-demo.yml
sed -i "s|SIGNALRPCINFO|$primaryConnectionString|" frontend-linkerd-demo.yml
sed -i "s|SIGNALRPCINFO|$primaryConnectionString|" daemon-linkerd-demo.yml

echo "Replaceing Image tags.."
sed -i "s/latest/$DockerImageTag/" backend-linkerd-demo.yml
sed -i "s/latest/$DockerImageTag/" frontend-linkerd-demo.yml
sed -i "s/latest/$DockerImageTag/" daemon-linkerd-demo.yml

echo "------------------Backend end-------------------------------"
cat backend-linkerd-demo.yml

echo "------------------Front end-------------------------------"
cat frontend-linkerd-demo.yml

echo "------------------daemon end-------------------------------"
cat daemon-linkerd-demo.yml

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "┊ Applying to Kubernetes                             │"
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"

kubectl apply -f namespace.yml

kubectl -n dapr-workload create secret docker-registry dockerhub --docker-server=https://index.docker.io/v1/ --docker-username=moimhossain --docker-password=$DockerPassword --docker-email=moimhossain@gmail.com

kubectl apply -f backend-linkerd-demo.yml
kubectl apply -f frontend-linkerd-demo.yml
kubectl apply -f daemon-linkerd-demo.yml