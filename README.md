# URL Shortener
## Setup Instructions
1. Download and install [.NET Core 2.1 SDK](https://www.microsoft.com/net/download)
2. Clone or download the UrlShortener project.
3. Open a command prompt and navigate to the folder that contains the .csproj file (**Not the *.sln!**).
4. Run the following commands:
   * dot net restore
   * dotnet ef database update *(after this, a sqlite database file should be created at the current location)*.
   * dotnet run
5. Make note of the port the server started and consume all the endpoints through the routes defined in the [specs](https://gist.github.com/fulcircle/0b7c84c17e1a9aa40c3d676e9cbfceb3).
