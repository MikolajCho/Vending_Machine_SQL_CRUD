🚀 Quick Setup Guide

Follow these steps to get the application up and running on your machine.
1. 🗄️ Database Preparation

First, Set up the database environment:

    Create the Database: Open SQL Server Management Studio (SSMS) and run the script provided in the file 
    "kod sql.txt".

    Install Dependencies: In Visual Studio, go to Manage NuGet Packages and install the following library:

        Microsoft.Data.SqlClient

2. ⚙️ Connection Configuration

Tell the application how to find your local SQL Server instance.

    Open the Program.cs file.

    Locate the line where the database service is initialized:

        static DatabaseService ds = new DatabaseService(".\\SQLEXPRESS", "vending_machine_71423");

    Update the parameters:

        Change the first parameter (.\\SQLEXPRESS) to the name of the SQL Server instance installed on your 
        computer.

        Keep the second parameter as the database name.

✅ You're all set! You can now build and run the project.
