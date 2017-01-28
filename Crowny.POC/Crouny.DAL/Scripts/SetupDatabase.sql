use [Master]

--Ensuring that Service Broker is enabled – replace the Database with the desired Database name

ALTER DATABASE Crouny SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE

GO 


--Switching to our database – replace the Database with the desired Database name

use Crouny


ALTER USER [IIS APPPOOL\DefaultAppPool]

WITH DEFAULT_SCHEMA = [IIS APPPOOL\DefaultAppPool]

GO 



IF NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE NAME='sql_dependency_subscriber' AND type ='R')

BEGIN

CREATE ROLE sql_dependency_subscriber AUTHORIZATION [IIS APPPOOL\DefaultAppPool]

END

GO


ALTER AUTHORIZATION ON ROLE::[sql_dependency_subscriber] TO [IIS APPPOOL\DefaultAppPool]

GO


--Permissions needed for [sql_dependency_subscriber]

GRANT CREATE PROCEDURE to [sql_dependency_subscriber]

GRANT CREATE QUEUE to [sql_dependency_subscriber]

GRANT CREATE SERVICE to [sql_dependency_subscriber]

GRANT REFERENCES on

CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]

  to [sql_dependency_subscriber]

GRANT VIEW DEFINITION TO [sql_dependency_subscriber] 


--Permissions needed for [sql_dependency_subscriber]

GRANT SELECT to [sql_dependency_subscriber]

GRANT SUBSCRIBE QUERY NOTIFICATIONS TO [sql_dependency_subscriber]

GRANT RECEIVE ON QueryNotificationErrorsQueue TO [sql_dependency_subscriber]

GRANT REFERENCES on

CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification]

  to [sql_dependency_subscriber] 


--Making sure that my users are member of the correct role.

EXEC sp_addrolemember 'sql_dependency_subscriber', 'IIS APPPOOL\DefaultAppPool'