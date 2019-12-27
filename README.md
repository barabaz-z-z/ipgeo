Launch PowerShell script `IPGeo/Solution Items/installer.ps1`
    It run publish API and Updater service (you can find `IPGeo Database Updater` in Windows Services), register service in Windows Services, run API.
    
In browser run `http://localhost:5000/api/ips/**{your IP}**/country` to get country details.
    First time it can show error related to database does not exist. Need to wait while Updater service downloads database and processes it.
