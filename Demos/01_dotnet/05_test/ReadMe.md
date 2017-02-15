## Net Core XUnit Tests

1. Add 'test' folder under 'src'.

2. Add "test" to "projects" in global.json.

3. Add 'libtests' folder to 'test'.
    - Change to libtests and run `dotnet new -t xunittest`

4. Run `dotnet restore`

5. If using VS Code, open folder above src directory.

6. Open project.json in libtests and add dependency to lib project. 

    ```
    "lib": {
        "target": "project"
    }
    ```

    - Run `dotnet restore` again.

7. Write a unit test for code in lib project.

    ```csharp
    [Fact]
    public void Echo_Should_Return_Upper_Case() 
    {
        // Arrange
        var input = "hello world!";
        var expected = "HELLO WORLD!";

        // Act
        var output = ClassLibrary.Hello.Echo(input);

        // Assert
        Assert.Equal(expected, output);
    }
    ```

8. Build libtests.

    ```
    cd tests/libtests
    dotnet build
    ```

9. Run tests from libtests directory.

    ```
    dotnet test
    ```

    - Alter code in test to pass or fail.
    - Run tests to see output.


