# HospitalManagementSystem
The first part of our Hospital Management System. This features the use of Code-First Migrations to create our database, and WebAPI and LINQ to perform CRUD operations.
# Scope
 ### contributed by Madiha Umair
  * Manage Locations, Departments information 
 * Manage Locations and associations between Locations and Departments

 ### contributed by Manali Patel
  * Manage Careers, Service information
 * Manage Services and associations between Locations and Services

 ### contributed by Udip Mandora
  * Manage News, Donation information

# Running this project
* Project > HospitalManagementSystem Properties > Change target framework to 4.7.1 -> Change back to 4.7.2
* Make sure there is an App_Data folder in the project (Right click solution > View in File Explorer)
* Tools > Nuget Package Manager > Package Manage Console > Update-Database
* Check that the database is created using (View > SQL Server Object Explorer > MSSQLLocalDb > ..)
* Run API commands through CURL to create new animals
# Common Issues and Resolutions
* (update-database) Could not attach .mdf database SOLUTION: Make sure App_Data folder is created
* (update-database) Error. 'Type' cannot be null SOLUTION: (issue appears in Visual Studio 2022) Tools > Nuget Package Manager > Manage Nuget Packages for Solution > Install Latest Entity Framework version (eg. 6.4.4), restart visual studio and try again
* (update-database) System Exception: Exception has been thrown by the target of an invocation POSSIBLE SOLUTION: Project was cloned to a OneDrive or other restricted cloud-based storage. Clone the project repository to the actual drive on the machine.
* (running server) Could not find part to the path ../bin/roslyn/csc.exe SOLUTION: change target framework to 4.7.1 and back to 4.7.2
* (running server) Project Failed to build. System.Web.Http does not have reference to serialize... SOLUTION: Solution Explorer > References > Add Reference > System.Web.Extensions
Make sure to utilize jsondata/department.json to formulate data you wish to send as part of the POST requests. {id} should be replaced with the animal's primary key ID. The port number may not always be the same

Get a List of Locations curl https://localhost:44324/api/locationdata/listlocations

Get a Single Location curl https://localhost:44324/api/locationdata/findlocation/{id}

Add a new Location (new location info is in location.json) curl -H "Content-Type:application/json" -d @location.json https://localhost:44316/api/locationdata/addlocation

Delete a Location curl -d "" https://localhost:44316/api/locationdata/deletelocation/{id}

Update an Location (existing location info including id must be included in location.json) curl -H "Content-Type:application/json" -d @location.json https://localhost:44324/api/locationdata/updatelocation/{id}
