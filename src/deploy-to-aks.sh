#!/bin/bash
DockerImageTag=$1

echo " "
echo "Preparing manifests with build Tag $DockerImageTag"

sed -i 's/latest/$DockerImageTag/' backend-linkerd-demo.yml
sed -i 's/latest/$DockerImageTag/' frontend-linkerd-demo.yml
sed -i 's/latest/$DockerImageTag/' daemon-linkerd-demo.yml

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

kubectl apply -f src/manifests/namespace.yml
kubectl apply -f src/manifests/backend-linkerd-demo.yml
kubectl apply -f src/manifests/frontend-linkerd-demo.yml
kubectl apply -f src/manifests/daemon-linkerd-demo.yml