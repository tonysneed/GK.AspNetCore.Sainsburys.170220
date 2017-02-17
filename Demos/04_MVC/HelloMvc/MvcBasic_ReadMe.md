# MVC Basic ReadMe

## Starter project for MVC Core web app

1. Add a global.json file to the solution

    - Links project to specific SDK version.

    ```json
    {
      "sdk": {
        "version": "1.0.0-preview2-1-003177"
      }
    }
    ```

2. Runtime: .NET Core 1.1

3. Tooling: Project.json

4. Contains:

    - Home ViewModel
    - Home Controller
    - Home Index View

5. Startup Options:

    - Visual Studio: Kestrel
        + Select MvcBasic profile
        + Press Ctrl+F5 or F5 (debugging)

    - Visual Studio: IIS Express
        + Select IIS Express profile
        + Press Ctrl+F5 or F5 (debugging)

    - Command Line: Watcher
        + Press Alt+Space to open command line
        + Run: `dotnet restore`
        + Run: `dotnet watch run`
        + Browse to: http://localhost:5000
        + Uses parameters in hosting.json
        + Recompiles and relaunches when any files change

