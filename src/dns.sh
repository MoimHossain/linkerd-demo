SubDomain="cluster"
ResourceGroup="rgp-foundations"
ZoneName="octo-lamp.nl"

echo "Retrieving IP of nginx Ingress..."
externalIP=$(kubectl get svc ingress-nginx-controller -n ingress-nginx -ojsonpath='{.status.loadBalancer.ingress[0].ip}')

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "┊ IP:  $externalIP                                  │"
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"

echo "Deleting existing A Records "
az network dns record-set \
    a delete --resource-group $ResourceGroup \
    --zone-name $ZoneName --name $SubDomain -y
echo "Creating A Record "    
az network dns record-set a add-record \
    --resource-group $ResourceGroup \
    --zone-name $ZoneName \
    --record-set-name $SubDomain --ipv4-address $externalIP

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "  Linkerd Dashboard: $SubDomain.$ZoneName  "
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"


SubDomain="myapp"
echo "Deleting existing A Records "
az network dns record-set \
    a delete --resource-group $ResourceGroup \
    --zone-name $ZoneName --name $SubDomain -y
echo "Creating A Record "    
az network dns record-set a add-record \
    --resource-group $ResourceGroup \
    --zone-name $ZoneName \
    --record-set-name $SubDomain --ipv4-address $externalIP

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "  App Dashboard: $SubDomain.$ZoneName  "
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"




SubDomain="bocanary"
echo "Deleting existing A Records "
az network dns record-set \
    a delete --resource-group $ResourceGroup \
    --zone-name $ZoneName --name $SubDomain -y
echo "Creating A Record "    
az network dns record-set a add-record \
    --resource-group $ResourceGroup \
    --zone-name $ZoneName \
    --record-set-name $SubDomain --ipv4-address $externalIP

echo " "
echo "┌┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┐"
echo "  Canary release DEMO URL: $SubDomain.$ZoneName  "
echo "└┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┘"
