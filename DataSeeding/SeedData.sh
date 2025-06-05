#!/bin/bash
echo "Seeding SeniorLearnDb..."
mongosh SLearnMobApp_db DataSeeding/SeedData.js
echo "Seeds Planted"