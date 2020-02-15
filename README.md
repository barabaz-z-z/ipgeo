# Task

Develop the web-service to define geo location by the user IP.

# Requirements
- The web-service stores all geographical data for every IP in PostgerSQL database.
- The web-service periodically updates mentioned above the database using some data provider, for example, MaxMind GeoLite2.
- The application for updating database is type of console.
- The web-service provides REST API to get data in JSON.

# Installation

Launch PowerShell script `IPGeo/Solution Items/installer.ps1`
    It run publish API and Updater service (you can find `IPGeo Database Updater` in Windows Services), register service in Windows Services, run API.
    
In browser run `http://localhost:5000/api/ips/**{your IP}**/country` to get country details.
    First time it can show error related to database does not exist. Need to wait while Updater service downloads database and processes it.
