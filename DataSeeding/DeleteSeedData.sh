#!/bin/bash

echo "Deleting Data in SeniorLearnDb..."
mongosh "mongodb://localhost:27017/SLearnMobApp_db" --file DataSeeding/DeleteSeedData.js
echo "Data Deleted"