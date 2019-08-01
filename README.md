# ArchitectProject
This project is a .Net core application.  To run the application, from the root directory of the repo run:
dotnet run --project ArchitectProject

By default the application will run on port 5001 (https://localhost:5001).

To obtain a new token, access the url:
https://localhost:5001/api/security/newToken?username=$USER_ID

Where $USER_ID is your user id.  In this version of the application, you may use any user id.