# Database upgrade tool
Light-weight tool for keeping track of your SQL database schema version

About
=====================

This is a database upgrade tool that works with .NET and SQL Server specifically. It is extremely light-weight: consists of only 4 classes (including Program.cs).

The best practices behind this tool are described [here][L1].

How to Get Started
--------------
To start following the database versioning best practices, you need to create a base-line script (the script containing all the objects your database has so far), place it to \Scripts as "01_Initial.sql" and execute the [Bootstrap.sql][L3] script on your database.

After that, you need to adjust the connection string 




Licence
--------------
[Apache 2 License][L2]



[L1]: http://enterprisecraftsmanship.com/2015/08/10/database-versioning-best-practices/
[L2]: http://www.apache.org/licenses/LICENSE-2.0
[L3]: src/DatabaseUpgradeTool/DddInAction.DB/DBSchema.txt
[L4]: DddInAction.Logic/Utils/Initer.cs