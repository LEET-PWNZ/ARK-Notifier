# ARK-Notifier
This tool was developed so that ARK: Survival Evolved unofficial server hosts can easily deploy a small system on their servers, that will allow them to capture and display notifications as generated by their ARK servers, so that the players on their servers can receive and view these notifications.  

I will describe how to configure and roll out this system, but I am assuming that the person reading this has some technical know-how regarding servers, development and hosting.  
The database environment used is MySQL so you'll need to have an instance installed.  
The application sends emails, so an SMTP server is required.  
The project uses DotNetOpenAuth (http://dotnetopenauth.net/) for retreiving the Steam IDs of players.  
The web application and service are both built on .Net 4.5    

The first requirement would be to get web notifications enabled on each instance of your ARK servers, read about it here: https://ark.gamepedia.com/Web_Notifications  
The ARK server will post it's notifications to the URL specified in the file 'AlarmPostCredentials.txt'  
The file that handles the posts from the ARK servers is located in the project root of the web app and named 'web_notif.aspx' so that has to be specified as the location of the URL.  
The tool was developed to handle hosts with multiple ARK instances, so you have to specify a server ID as a query string parameter, and this ID needs to be unique for each of your ARK instances.
Your file contents for 'AlarmPostCredentials.txt' should look something like below:  

your_server_key  
http://your.web_notification.host/web_notif.aspx?server_id=1  

Once you have your ARK servers configured to post notifications, you are ready to set-up and deploy the web application, so first you'll need to download this project, and then you can open it in Visual Studio.  
The first thing to do, is set up the database. There is an empty database in the project root containing the required schema only.  
Once this has been restored / replicated in your own MySQL environment, you need to set up your ARK servers in the table named 'servers'. The 'server_id' query string variable (AlarmPostCredentials.txt) that will be posted from your ARK server(s) will need to correspond to the 'id' field of this table. The 'server_name' field is just for players to identify the server from which they received the notification, so the admins need to set this accordingly.  
You must now set-up the application to utilize your MySQL server with the newly restored database.  
To do this, open the solution in Visual Studio and make note that there are two projects:  

Project 1: ARK Notifier Service  
This project is configured as a Windows service, and it handles the emailing as well as the purging of old sessions.  
The project has an 'App.config' file containing the SMTP settings and the MySQL connection string, so be sure to set these up based on your hosting environment.  
In the 'app.config' file there is a setting named 'WebAppHostname'. This needs to be set as the URL that the emailer will use to direct links toward, so if your app is hosted on my.domain.com/ark-notifier/, then that full URL and path needs to be specified in this setting, so that when a user requests to have their password reset, the email will contain a link pointing to http://my.domain.com/ark-notifier/ResetPassword.aspx  
The other settings names' are self-explanatory.  
  
Project 2: ARK Web Notifier  
This project is the actual web app that players will use to register on, and view notifications. It also contains the file named 'web_notif.aspx' which is where the posts from your ARK servers will be submitted.  
There is a 'web.config' file in this project containing a MySQL connection string. This would need to point to the same MySQL server and database used above.  
There is also a setting named 'ArkPostKey', which needs to match the key specified in your 'AlarmPostCredentials.txt' file on the first line.  
In the file 'Classes\Globals.cs' there are 2 variables named 'cryptoSalt' and 'cryptoSecurityKey'. These are used for encryption of the cookie contents and for encrypting the passwords stored in the database, so I would recommend that you set your own unique values for these variables to maximize your security.  
  
Once you have your MySQL and SMTP server settings specified in the 'web.config' and the 'app.config' files of the above projects, then you should be ready to start using the web notifications.  
This entire system is extremely minimalistic and small, so anyone should be able to figure it out and customize it even further with ease.  

For users to make use of this system, they will need to register on the application first.  
Once registered, the application will send an email to the address provided by the user, and the user will need to follow the link in this email to verify their accounts.  
Once a user has verified their account, the application will request that they link their Steam ID by logging in via the Steam website.  
Once the Steam ID has been linked to their accounts, they will be able to view any notifications that were pushed out for that Steam ID.  
Note that for tribes, a notification will be pushed out for the Steam ID of every tribe member.  
A working (But modified) instance of this application can be found here: http://leetpwnz.elitedigital.co.za/ark-notifier/  
Naturally, this instance of the application will only work for people that play on the LEET PWNZ servers (http://leetpwnz.elitedigital.co.za)  
