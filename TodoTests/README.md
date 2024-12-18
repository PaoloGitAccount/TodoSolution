# TodoTests

## Overview

The `TodoTests` project contains unit tests for the `TodoSolution` application, ensuring the functionality and reliability of various components. The tests are written using the xUnit framework and include mock objects using Moq.

## Structure

The `TodoTests` project follows a simple structure:

### TodoTests.csproj

The project file for the test project, which includes references to necessary packages such as xUnit and Moq.

### UnitTests/

This directory contains all unit test classes. In this example, we have:

- **TodoServiceTests.cs**: Contains tests for the `TodoService` class.

## Dependencies

The project relies on the following NuGet packages:

- **xUnit**: A free, open-source, community-focused unit testing tool for .NET.
- **xUnit.runner.visualstudio**: Provides Visual Studio integration for xUnit.net.
- **Moq**: The most popular and friendly mocking framework for .NET.

## How to Run Tests

To run the tests in the `TodoTests` project, follow these steps:

1. **Using Command Line**:

   Navigate to the `TodoTests` directory and run the following command:

   ```sh
   dotnet test
