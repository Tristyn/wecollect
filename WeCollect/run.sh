npx iisexpress-proxy 5000 to 80 &
npx iisexpress-proxy 10000 to 10004 &

"C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator\AzureStorageEmulator.exe" start 

"C:\Program Files\Azure Cosmos DB Emulator\Microsoft.Azure.Cosmos.Emulator.exe"

echo ""
echo ""
echo "Startup complete."
echo "You must manually start Ganache and copy your hot wallet key generated by Ganache to appsettings.json"
echo "Application is listening on port 80 and image CDN on port 10004. Please add firewall rules and forward these ports."