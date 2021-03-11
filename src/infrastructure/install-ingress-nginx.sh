#!/bin/bash

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