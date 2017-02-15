## Publishing a .NET Core App

### Self-Contained apps

1. Omit "type":"platform" from framework dependency in project.json.

2. Add "runtimes" to project.json

    ```json
    "runtimes": { 
        "win10-x64": {}, 
        "osx.10.12-x64": {},
        "ubuntu.14.04-x64": {}
    }
    ```

3. Run `dotnet restore`.

4. Build with runtime outputs.

    ```
    dotnet build -r win10-x64
    dotnet build -r osx.10.12-x64
    dotnet build -r ubuntu.14.04-x64
    ```

5. Publish with runtime outputs.

    ```
    dotnet publish -r win10-x64
    dotnet publish -r osx.10.12-x64
    dotnet publish -r ubuntu.14.04-x64
    ```

6. Copy to target VM and execute.
    - On MacOS and Linux:
        + First run: `chmod u+x app`
        + Then run: `./app`