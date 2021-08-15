# Crossroad - Backend

Find your favorite titles using your favorite movies search engine - Crossroad!

This is the backend portion of the project. This WebApi project interacts with the SQL Server database for titles that was provided. 

The Angular 12 frontend consumes this WebApi in order to allow visitors to find their favorite titles in an instant!

Run this project first before running Angular frontend!

## Running WebAPI project .Net Core

Once you download source code, simply make sure that you have .Net Core 3.1 installed in your local machine before running project. 

Secondly, ensure you update the connection string to the dataabase on the appsettings.json file to point to your own local database instance.

## CORS

WebAPI project already has all the necessary configuration to enable CORS HTTP requests to happen between the angular frontend and the WebApi project.

There shoulnd't be any issue consuming the end points from the Angular Crossroad application.

## Tech Stack

.Net Core 3.1\
Entity Framework 3.1 Code First Approach

## About End Points

All data returned from the end points is in JSON format ready to be consumed by the Angular Client. The following end points where created:

1. GetTitleById
2. SearchByTitle
3. GetTitleDetails

The current functionality is limited to SearchByTitle (the only one currently in use by the Angular frontend). 

The other 2 end points work but they were created for the purpose of future implementations. 

For instance the **GetTitleDetails** end point will be useful for displaying the detailed information about a specific title. 

![Lazy Loading All Info In One Single Call](https://github.com/Steel9561/Crossroad-Titles/blob/daa49735625a40f69ed2df4663569b1f25d64016/Crossroad-API/Crossroad-API/GetTitleDetails.JPG)

This is because this end point will bring back all related records to a specific title (awards, other names, participants and genres). 

For best performance, all the data for a specific title is returned in one single call using lazy loading. 


