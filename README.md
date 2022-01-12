# ASPNET_Project_Baespeaks.com
A website I created with ASP.NET Core using the content from my blog baespeaks.com

- Target Framework is NET 6
- The first time you attempt to run the program there might be an error saying that a file cannot be found under the /obj folder. If you try running it again, the issue does not persist
- One user login is daniel@baespeaks.com and the password is "asdfasdf". This login has admin privileges
- Password Reset and Email Confirmation links are sent to the log file called LogFile.txt which is in the root directory
- The Id property is set to 0 before passing the view model to the client when Id encryption is used for route parameters
- The following NuGet packages are used: bootstrap 5.1.2, Microsoft.AspNetCore.Identity.EntityFrameworkCore 6.0.0, Microsoft.EntityFrameworkCore 6.0.0, Microsoft.EntityFrameworkCore.Design 6.0.0, Microsoft.EntityFrameworkCore.Sqlite 6.0.0, Microsoft.EntityFrameworkCore.Tools 6.0.0
- "ASPNETCORE_ENVIRONMENT" is set to "Production"
- Within the site there is a Portfolio section which has a handful of console applications made with C#, which can also be found at baespeaks.com/portfolio
- The name of the project is ASPNET_Project_Eleven though the folder name is ASPNET_Project_Ten. The code is executable though renaming the folder may cause it to not work

Known Issues:
- The type Blueprint under Models has Year, Month, and Date, though they are not used
- If you try and make an article with a date combination that is invalid, the article will be created though the article cannot be viewed as the DateTime construction with invalid parameters will throw an exception
