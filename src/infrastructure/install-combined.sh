#!/bin/bash


echo "Installing Linkerd core (data and control plane)"

curl -sL https://run.linkerd.io/install | sh
export PATH=$PATH:$HOME/.linkerd2/bin
linkerd check --pre
linkerd install | kubectl apply -f -
linkerd check

echo "Waiting for 2 mins to give Linkerd time to get things worked out..."
sleep 2m
echo "Installing linkerd viz"

linkerd viz install | kubectl apply -f -

linkerd viz check
linkerd check

echo "Waiting for 2 mins to give Linkerd Viz time to get things worked out..."
sleep 2m
echo "Installing Ingress nginx"

helm repo add stable https://charts.helm.sh/stable
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
kubectl apply -f ./namespace.yml

helm install ingress-nginx ingress-nginx/ingress-nginx \
    --namespace ingress-nginx \
    --set controller.replicaCount=2 \
    --set controller.metrics.enabled=true \
    --set controller.podAnnotations."prometheus\.io/scrape"=true \
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux \
    --set controller.podAnnotations."prometheus\.io/port"=10254 \
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux


echo "Waiting for 4 mins to give ingress-nginx time to get things worked out..."
sleep 4m
kubectl get all -n ingress-nginx


echo "Creating ingress for Linkerd viz"
kubectl apply -f ./dashboard.yml