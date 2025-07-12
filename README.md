<h1 style="text-align: center;">.NET Project "Forum-System" </h1>
<p <p align="center">
<img src=https://i.ibb.co/S3G4t8N/mushroom.png width="100%" height="100%"/>
</p>

## Overview

An forum website where users can discuss video games.

---
<br>

## Features

<h3>Anonymous users can see the home page where they can:</h3>

+ See the a list of the latest 10 threads that have been created with the number of comments and it's overall rating score
+ See how long ago they have been created
+ See the author of the post
+ See the 10 most commented threads below
+ On the right-hand side they can see:
  * The total number of posts creted so far
  * The total number of users so far
  * The latest comment that has been created and it's author's name
 
+ They have access to the About information
+ And the API test page where they can test making requests using the Sqagger UI
+ Register and create a new account

<h3>Logged Users can see all that the Anonymous users can see plus:</h3>

+ See user avatars on the home page
+ Create new threads with title and a initial message
+ Leave comments on already existing threads 
+ Rate any thread with a rating from 0.5 to 5
+ Remove their rating or change it at any time
+ Leave Comments in threads
+ Edit or delete their own threads and comments
+ See a list of all threads in the forum
+ Sort this list by Rating Comments Date and Title
+ See any user profile by clicking on the user name
+ See all user profiles from the button Users in the navigation bar

<br />

## Technologies Used

- Entity Framework 6.0
- Bootstrap library
- Necessary packages:
  + Microsoft.AspNetCore.Authentication.JwtBearer Version="6.0.25"
  + Microsoft.AspNetCore.Mvc.NewtonsoftJson Version="6.0.25"
  + Microsoft.EntityFrameworkCore.Design Version="6.0.25"
  + Microsoft.EntityFrameworkCore.SqlServer  Version="6.0.25"
  + Microsoft.EntityFrameworkCore.Tools Version="6.0.25"
  + Swashbuckle.AspNetCore Version="6.5.0"
  + Swashbuckle.AspNetCore.Annotations Version="6.5.0"
  + System.IdentityModel.Tokens.Jwt Version="6.35.0"
- Tests Necessary packages:
  + MOQ Version="4.20.70"

---

## Installation
The project generates a database using code first approach. 

Follow these steps to set up and run the application:


1. **Step one**
- Install all necessary packages using the the NuGet Package manager in Visual Studio 2023
- (You can omit MOQ if you don't need to run tests)

2. **Step two**
- Setup the SQL server connection to your computer by creating an appsettings.json file in root folder of the project (/Forum_System/)
  
3. **Step three**
- Paste this code in your appsettings.jon file:

```
{
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server={{YourServerconnectionString}};Database=ForumDatabaseProject;Trusted_Connection=True;"
  },

  "Jwt": {
    "Key": "MySecretKeyForAuthenticationOfApplication",
    "Issuer": "CompanyIssuer.com"
  }
}
``` 
You need to replace ```{{YourServerconnectionString}}``` with your actual SQL connection string. 


4. **Step four**
- Open the Package Manager Console and create a new database migration.
```
Add-Migration Initial
```
5. **Step five**
- In the console upload the migration
```
Update-Database
```
6. **Step six**
- Build the project

7. **Step seven**
- Open it in a browser
The url with the port you are using is set in /Properties/launchSettings.json:
this line:
```"applicationUrl": "http://localhost:5000"```
This is the url you can use to access the Forum after you have build the project. 

<br>

#### Home Page - before and after login


Before Login:

<img src=https://i.ibb.co/WcTq689/Before-Login.png />

After Login:

<img src=https://i.ibb.co/34xfbGB/After-Login.png />

#### Login Page 

<img src=https://i.ibb.co/0t5xGqY/loginscreen.png />

#### API
<img src=https://i.ibb.co/23GdYx7/api-1.png />
***
<img src=https://i.ibb.co/XC14jbQ/api-2.png />

#### Browse User Profile

+ Navigate to the user list
<img src=https://i.ibb.co/pJxL9mp/menu-Users.png />
***
<img src=https://i.ibb.co/WzK6YNp/Users-List.png />

#### List of all threads

+ Navigate to the threads list
<img src=https://i.ibb.co/ThWHfGM/menu-Threads.png />
***
<img src=https://i.ibb.co/8YdRtf7/Threads-list.png />

#### Create thread

+ Navigate to the home page
<img src=https://i.ibb.co/DQjmjgx/menu-create-Theread.png />

+ Click the hand with the pencil
<img src=https://i.ibb.co/PwTZWhP/Create-Thread-2.png />

+ Fill the field and click Post
<img src=https://i.ibb.co/fndL0CY/create-thread-3.png />

#### Add comment

+ Navigate to the thread where you want to leave a comment
<img src=https://i.ibb.co/sQTt9w7/navigate-to-thread.png />

+ Click on the comment button
<img src=https://i.ibb.co/7Vp6pDK/navigate-to-button-Comment.png />

+ Type your comment and click Post
<img src=https://i.ibb.co/W3WpKDw/leave-a-comment.png />

+ Your comment appears at the bootm of the list
<img src=https://i.ibb.co/tqdFpxT/comment-2.png />

#### Rate thread

+ You can also rate any thread
<img src=https://i.ibb.co/WWDvnkB/Rate-1.png />

#### Edit and Delete Buttons
<img src=https://i.ibb.co/Yc3NH9P/comment-1.png />

<br />


## Database Diagram

<img src=https://i.ibb.co/6B49Nng/database-diagram.png />

<br>


---

## Contributors
{{For further information, please feel free to contact us: - or some other message}}

| Authors         | Emails                                        | GitHub                               |
| ------          | ------                                        |------                                |
| Tomi Tsolov     | tvtsolov@gmail.com                            | [link](https://github.com/tvtsolov)  |
| Stanimir Nenkov | stanimir.nenkov.a54@learn.telerikacademy.com  | [link](https://github.com/stnmrr)    |

---
<br />

