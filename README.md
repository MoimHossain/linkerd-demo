# LinkerD Demo application on AKS

This repository contains a simple application written in .net core that runs on Linux container on Azure Kubernetes Service and uses LinkerD service mesh to demonstrate the service mesh proxies that intercept gRPC inter-service calls, deploying mTLS, observability etc.

# How to run?
The Azure pipeline (azure-pipeline.yml) has all the necessary commands that will create a AKS cluster, install LinkerD on it and then build the docker images for 3 services, push them in Docker public registry and then deploy them to AKS.

To run locally, you can execute the bash files, specially buildimages.sh and deploy-to-aks.sh.
