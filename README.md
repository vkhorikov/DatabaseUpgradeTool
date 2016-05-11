# Database upgrade tool
Light-weight tool for keeping track of your SQL database schema version

About
=====================

This is a database upgrade tool that works with .NET and SQL Server specifically. It is extremely light-weight: consists of only 4 classes (including Program.cs).

The best practices behind this tool are described [here][L1].

How to Get Started
--------------
To start following the database versioning best practices, you need to create a baseline script (the script containing all the objects your database has so far), place it to [Scripts][L5] as "01_Initial.sql" and execute [Bootstrap.sql][L3] on your database.

After that, you need to adjust the [connection string][L4] and you are good to go.

How to Use the Tool
--------------
The best way to use the tool is to copy it to your solution as a separate project:

![Project structure](https://lh3.googleusercontent.com/jDln5bJl0gwyhZ-LERxybb4A4CK4J6Kapm51gmHlOIE=w273-h280-no)

After that, you can add new migrations to the Scripts folder and execute them just by hitting F5:

![Running the application](http://i.imgur.com/wJBhj3T.png)

**Warning!** Don't forget to mark the files in the Scripts folder as Content, Copy Always:

![Running the application](https://lh3.googleusercontent.com/zSVmry_etu7gbmCE87E_-BxJAhIHY9_SzM0QH38tsNI=w336-h244-no)

What if one of the scripts fails?
--------------
The tool executes the scripts in a transactional manner. If for example migration #4 contains an error which results in an exception, the tool will execute migrations 1, 2, and 3; migration 4 will be rollbacked entirely. The database would be marked as of version 3.

Licence
--------------
[Apache 2 License][L2]

[L1]: http://enterprisecraftsmanship.com/2015/08/10/database-versioning-best-practices/
[L2]: http://www.apache.org/licenses/LICENSE-2.0
[L3]: src/DatabaseUpgradeTool/Bootstrap.sql
[L4]: src/DatabaseUpgradeTool/App.config
[L5]: src/DatabaseUpgradeTool/Scripts