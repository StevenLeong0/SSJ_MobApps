#!/bin/bash
echo "Seeding SeniorLearnDb..."
mongosh SLearnMobApp_db SeedData.js
echo "Seeds Planted"